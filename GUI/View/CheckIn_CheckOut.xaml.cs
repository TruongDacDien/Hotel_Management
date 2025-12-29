using System;
using System.Text.RegularExpressions;
using System.Windows;
using BUS;
using DAL.DTO;

namespace GUI.View
{
    /// <summary>
    /// Interaction logic for CheckIn_CheckOut.xaml
    /// </summary>
    public partial class CheckIn_CheckOut : Window
    {
        private readonly bool isCheckIn;
        private readonly int? maCTPT;
        private readonly LoaiPhong loaiPhong;
        private readonly Phong_Custom phong;

        // Cho phép ± 60 phút so với giờ chính sách
        private static readonly int AllowedWindowMinutes = 60;

        public CheckIn_CheckOut(bool isCheckIn, Phong_Custom phong, LoaiPhong loaiPhong)
            : this()
        {
            this.isCheckIn = isCheckIn;
            this.phong = phong;
            this.maCTPT = phong?.MaCTPT;
            this.loaiPhong = loaiPhong;
            LoadUI();
        }

        private CheckIn_CheckOut()
        {
            InitializeComponent();
        }

        private void LoadUI()
        {
            txblTitle.Text = isCheckIn ? "Check-in" : "Check-out";
            txbPhong.Text = phong?.MaPhong ?? string.Empty;
            txbKhachHang.Text = phong?.TenKH ?? string.Empty;
            txbChinhSach.Text = loaiPhong?.ChinhSach ?? "Không có chính sách";
            txbThoiGian.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void click_XacNhan(object sender, RoutedEventArgs e)
        {
            if (!maCTPT.HasValue || maCTPT.Value <= 0)
            {
                new DialogCustoms(
                    "Không tìm thấy mã chi tiết phiếu thuê!",
                    "Thông báo",
                    DialogCustoms.OK
                ).ShowDialog();
                return;
            }

            var now = DateTime.Now;

            // 1) Kiểm tra khoảng thời gian hợp lệ theo chính sách (±1 tiếng)
            if (
                TryParsePolicyTimes(
                    loaiPhong?.ChinhSach,
                    out var policyCheckIn,
                    out var policyCheckOut
                )
            )
            {
                var actual = now.TimeOfDay;
                var target = isCheckIn ? policyCheckIn : policyCheckOut;

                if (!IsWithinMinutesWindow(actual, target, AllowedWindowMinutes))
                {
                    var actionText = isCheckIn ? "nhận phòng (check-in)" : "trả phòng (check-out)";
                    var allowedWindowText = FormatAllowedWindow(target, AllowedWindowMinutes);

                    new DialogCustoms(
                        $"Bạn chỉ được {actionText} trong khoảng {allowedWindowText}.\n"
                            + $"Chính sách hiện tại: {FormatPolicyTimes(policyCheckIn, policyCheckOut)}.",
                        "Thông báo",
                        DialogCustoms.OK
                    ).ShowDialog();
                    return; // CHẶN cập nhật DB
                }
            }
            // Nếu không parse được chính sách => bỏ qua kiểm tra (tùy bạn; muốn chặn thì đổi logic tại đây)

            var ghiChu = txbGhiChu.Text;
            var error = string.Empty;

            // 2) Cập nhật DB
            var isOk = isCheckIn
                ? CT_PhieuThueBUS.GetInstance().capNhatCheckIn(maCTPT, now, ghiChu, out error)
                : CT_PhieuThueBUS.GetInstance().capNhatCheckOut(maCTPT, now, ghiChu, out error);

            if (!isOk)
            {
                new DialogCustoms("Lỗi: " + error, "Thông báo", DialogCustoms.OK).ShowDialog();
                return;
            }

            // 3) Vẫn có thể thông báo sớm/trễ so với giờ chính sách (nhưng đã nằm trong ±1 tiếng)
            ShowPolicyNotice(now);

            DialogResult = true;
            Close();
        }

        private void ShowPolicyNotice(DateTime actualTime)
        {
            if (
                !TryParsePolicyTimes(
                    loaiPhong?.ChinhSach,
                    out var checkInTime,
                    out var checkOutTime
                )
            )
                return;

            var actual = actualTime.TimeOfDay;

            if (isCheckIn)
            {
                if (actual < checkInTime)
                {
                    new DialogCustoms(
                        $"Nhận phòng sớm hơn chính sách ({FormatPolicyTimes(checkInTime, checkOutTime)}).",
                        "Thông báo",
                        DialogCustoms.OK
                    ).ShowDialog();
                }
                else if (actual > checkInTime)
                {
                    new DialogCustoms(
                        $"Nhận phòng trễ hơn chính sách ({FormatPolicyTimes(checkInTime, checkOutTime)}).",
                        "Thông báo",
                        DialogCustoms.OK
                    ).ShowDialog();
                }
            }
            else
            {
                if (actual > checkOutTime)
                {
                    new DialogCustoms(
                        $"Trả phòng muộn hơn chính sách ({FormatPolicyTimes(checkInTime, checkOutTime)}).",
                        "Thông báo",
                        DialogCustoms.OK
                    ).ShowDialog();
                }
                else if (actual < checkOutTime)
                {
                    new DialogCustoms(
                        $"Trả phòng sớm hơn chính sách ({FormatPolicyTimes(checkInTime, checkOutTime)}).",
                        "Thông báo",
                        DialogCustoms.OK
                    ).ShowDialog();
                }
            }
        }

        private static bool TryParsePolicyTimes(
            string policy,
            out TimeSpan checkIn,
            out TimeSpan checkOut
        )
        {
            checkIn = default;
            checkOut = default;
            if (string.IsNullOrWhiteSpace(policy))
                return false;

            // Lấy 2 mốc HH:mm đầu tiên trong chuỗi chính sách
            var matches = Regex.Matches(policy, @"\b([01]?\d|2[0-3]):[0-5]\d\b");
            if (matches.Count < 2)
                return false;

            return TimeSpan.TryParse(matches[0].Value, out checkIn)
                && TimeSpan.TryParse(matches[1].Value, out checkOut);
        }

        private static string FormatPolicyTimes(TimeSpan checkIn, TimeSpan checkOut)
        {
            return $"{checkIn:hh\\:mm} - {checkOut:hh\\:mm}";
        }

        // --------- Helpers: ± minutes window ---------

        private static bool IsWithinMinutesWindow(TimeSpan actual, TimeSpan target, int minutes)
        {
            // So sánh theo phút trong ngày, có xử lý vòng 24h (đề phòng gần 00:00)
            var a = (int)Math.Round(actual.TotalMinutes);
            var t = (int)Math.Round(target.TotalMinutes);

            var diff = Math.Abs(a - t);
            diff = Math.Min(diff, 24 * 60 - diff);

            return diff <= minutes;
        }

        private static string FormatAllowedWindow(TimeSpan target, int minutes)
        {
            var start = NormalizeTime(target - TimeSpan.FromMinutes(minutes));
            var end = NormalizeTime(target + TimeSpan.FromMinutes(minutes));
            return $"{start:hh\\:mm} - {end:hh\\:mm}";
        }

        private static TimeSpan NormalizeTime(TimeSpan time)
        {
            // Đưa về [0, 24h)
            var day = TimeSpan.FromDays(1);
            var ticks = time.Ticks % day.Ticks;
            if (ticks < 0)
                ticks += day.Ticks;
            return TimeSpan.FromTicks(ticks);
        }

        private void click_Thoat(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
