Update wallets/balance and transactions in two ways:
- Via timer, you can set delay in seconds in appsettings.
- Via notifications from bitcoind, for quick response.

Check:
1. Setup bitcoind regtest (https://bitcoin.org/en/developer-examples)
2. Put "Files/bitcoin.conf" to "c:\Users\*YOU_USER*\AppData\Roaming\Bitcoin\"
3. Put blocknotify.ps1 and walletnotify.ps1 to "c:\My\" as mentioned in bitcoin.conf
4. Default address is localhost:50000, If you want to change it you should change in Project Properties and also in blocknotify.ps1, walletnotify.ps1, To make correct notifications.
5. Set connection string in appsettings.
6. Set bitcoind params in appsettings. (default address for bitciond http://127.0.0.1:18443)

WebApi:
- GetLast: GET http://localhost:50000/api/bitcoin/getlast
- SendBtc: POST http://localhost:50000/api/Bitcoin/SendBtc?address=2NCEZmYTnzCDdpGqxN5UyyN7XM5REA3AREL&amount=1&fromWallet=
address - address btc receiver
amount - amount of btc
fromWallet - (optional) from what wallet to send btc
- BlockNotify: GET http://localhost:50000/api/notify/block
Instant update of Wallets and Balance, and Transactions
- WalletNotify: GET http://localhost:50000/api/notify/wallet?txid=1dc5e5a3ba5da856b6f92655503c3693d27237f9e644c98c9607c24bf7a9376a
Instant update of Transactions
txid - Id of new transaction