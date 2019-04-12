param(
    # Parameter help description
    [Parameter(Mandatory = $true)]
    [string]
    $Token,

    [Parameter(Mandatory = $true)]
    [string]
    $Url
)

$setWebhookUrl = [string]::Format("https://api.telegram.org/bot{0}/setWebhook", $Token);
$params = @{
    url = $Url
}

Invoke-WebRequest -Method Post $setWebhookUrl -Body $params