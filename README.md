# StakeMaster
Tool to manage stakin crypto wallets.

## Prerequisites
This tool make use of the following rpc functions. The wallet have to support this functions for the tool to work. The functions also have to support the stated parameters in the exact order, additional parameters that are present eventually must have a default value or the tool will not work.
* createrawtransaction [{"txid":"id","vout":n},...] {"address":amount,...}
* getstakesplitthreshold
* gettransaction "txid"
* listreceivedbyaddress
* listunspent listunspent minconf maxconf ["address",...]
* sendrawtransaction "hexstring"
* setstakesplitthreshold value
* signrawtransaction "hexstring"
* walletlock
* walletpassphrase "passphrase" timeout anonymizeonly

The wallet needs to have a password set. So if you want to use this tool it will force you to encrypt your wallet with a password. It's for your own safety trust me.

## First run
Bevore the first run you should check the settings in appsettings.json.
### TransactonSettings
The predefined values are valid for MUE, SNYX and ESBC wallets, other wallets are not tested.
#### InputSize
The size one input is using inside a transaction. To calculate the input size open your wallet go to the coin control window and select only one input and then only two inputs. The input size is the difference between the shown bytes.

Example based on the MUE wallet:

	Inputs selected: 1 Bytes shown: 226
    Inputs selected: 2 Bytes shown: 374
    InputSize = 374-226 = 148
#### OutputSize
The size one output is using inside a transaction. To calculate the output size open your wallet go to the coin control window and select only one input. Close the window and you are at the send window where the current bytes for the transaction is shown. Now check UTXO and split into 2 outputs. The shown bytes will change. The output size is the difference between the shown bytes.

Example based on the MUE wallet:

	Inputs selected: 1 UTXO: unchecked Outputs: Empty Bytes shown: 226
    Inputs selected: 2 UTXO: checked Outputs: 2 Bytes shown: 260
    InputSize = 260-226 = 34
#### Overhead
The size one transaction needs for the internal data structure. The overhead can be calculated when substracting the input size and output size from the transaction size. The wallets assume that every transaction has at least two outputs (one output for the target address and one output for the change) so you can start calculation when you know the size of a transaction with one input.

Example based on the MUE wallet:

	Inputs selected: 1 UTXO: unchecked Outputs: Empty Bytes shown: 226
    Overhead = 226-(inputSize+2*OutputSize) = 226-(148+68) = 226-216 = 10
#### FreeTransactionSizeLimit
Just look it up in the specification of the coin.
#### RpcTimeoutInSeconds
The time the tool will wait till the walled response to the rpc call. You can adjust that value to your needs.
#### BaseDateOfTransactions
The time inside blocks or transactions are saved as seconds since a specific date. For most wallet this will be `1970-01-01`. However there are some wallets that use an alternative date. Please consult the specification of the coin for the actual date. Alternatively you can look up the date value of the last block and calculate the date yourself. Do not forget that the dates are UTC.
### Serilog
These are the settings for the screen logging. You should leave it as it is unless you really know what you are doing. It is save to change `outputTemplate` and `MinimumLevel`. For more information consult [Serilog homepage](https://serilog.net/).
#### MinimumLevel
This setting controls which ouputs are shown on the screen.

Possible values are
* Verbose (Very detailed output. Only useful to pinpoint an bug)
* Debug (Output for the curious kind of people)
* Information (Default. Shows a decent amount of information)
* Warning (Shows only warning messages and higher)
* Error (Shows only error messages and higher)
* Fatal (Shows only fatal messages)

## Running the tool
The tool is a portable dotnet core application and can be run on every system dotnet core 2.1 is available. This includes windows, linux and mac. To simply show a list of available commandline parameters just run `dotnet StakeMaster.dll` on your command line.

### Commandline parameters
#### General settings
`-?` or `--help`: Displays the help

#### Settings regarding the stake function
`-s=<true/false>` or `--stakes=<true/false>`: Enables or disables modifications of inputs at the stake address. Default: `true`

` -a=<address>`  or `--stakeaddress=<addres>`: Sets the dedicated stake address. Mandatory.

`-c=<address>` or `--collectaddress=<address>`: Sets the dedicated collect address. Mandatory.

`-w=<days>` or `--patience=<days>`: Sets the number of days when a input should stake at least once. Default: `7`

`-q=<password>` or `--walletpassword=<password>`: Sets the password for the wallet. Mandatory

#### Settings regarding all other addresses in the wallet
`-i=<true/false>` or `--collectinputs=<true/false>`: Moves all inputs from other addresses to the collect address. Default: `true`

` -e=<adr_1>{,<adr_n>}`  or ` --excludeaddress=<adr_1>{,<adr_n>}`: Comma seperated list of addresses that will be excluded.

#### Settings regarding the rpc connection to the wallet
`-u=<user>` or `--user=<user>`: The user for the rpc connection. Mandatory.

`-p=<password>` or `--password=<password>`: The password for the rpc connection. Mandatory.

`-o=<uri>` or `--uri=<uri>`: The uri for the rpc connection. Mandatory.
## Additional Notes
Personally I prefer to call the tool once a day to keep my wallets in check.

If you think the tool is great you can tip me

	MonetaryUnit         : 7jazUQfTAZ9EafMwcieZ4gRRY6HLVzNfTx (best coin ever)
	Bitcoin              : 1JoF1MuzdFUfQF4cr9g7X4sGdEkf7DTxQM
	Syndicate            : SdXXExhpANigxrvxjKfC89V2h7EMo5UYEF
	E-Sports betting coin: Ec85UETPMq2smXR9hKfRFyPnVJE8G739Tf
