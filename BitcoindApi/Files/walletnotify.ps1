param 
( 
    [string]$txid = ""
)

Invoke-WebRequest http://localhost:50000/api/notify/wallet?txid=$txid