# Define the current directory for file paths
$currentDir = Get-Location

# Path to ngrok.exe
$ngrokPath = "$currentDir\ngrok.exe"

# Check if ngrok.exe exists
if (-Not (Test-Path $ngrokPath)) {
    Write-Output "error=Ngrok executable not found"
    Exit 1
}

# Add auth token if not already added
$authToken = "2qJDNlekUCaGf01G78B99Vrb4Pa_4939xqxnEPz2DEMhrR2q2"
$ngrokConfigPath = "$HOME\.ngrok2\ngrok.yml"

if (-Not (Test-Path $ngrokConfigPath -and (Get-Content $ngrokConfigPath) -match $authToken)) {
    Start-Process $ngrokPath -ArgumentList "config add-authtoken $authToken" -NoNewWindow -Wait
}

# Start Ngrok in hidden mode
Start-Process $ngrokPath -ArgumentList "http https://localhost:7049" -WindowStyle Hidden

# Wait for Ngrok to initialize (latency check with retries)
$maxRetries = 10
$retryDelay = 2
$retryCount = 0
$ngrokUrl = $null

while ($retryCount -lt $maxRetries) {
    try {
        $response = Invoke-RestMethod -Uri "http://127.0.0.1:4040/api/tunnels" -Method Get
        if ($response.tunnels.Count -gt 0) {
            $ngrokUrl = $response.tunnels[0].public_url
            break
        }
    } catch {
        # Ignore errors and continue retrying
    }
    Start-Sleep -Seconds $retryDelay
    $retryCount++
}

if (-Not $ngrokUrl) {
    Write-Output "error=Failed to retrieve Ngrok URL"
    Exit 1
} else {
    Write-Output "cancelUrl=$ngrokUrl"
    Write-Output "returnUrl=$ngrokUrl"
}
