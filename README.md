# MasterOfLegend

Read NFTs from MasterOfLegends collection

<h4> Unity project </h4> 

This project is a testing project to read NFT(ERC1155 tokens) from user's wallet.
https://opensea.io/collection/masteroflegends <br>
If you bought NFT from this collection, you can see your token in game.


- Implemented functions

    1. Sign in using Metamask, Coinbase Wallet, TrustWallet, SafePal
    2. Read ERC1155 tokens from user's wallet
    3. Read metadata from the token IDs
    4. List all tokens in Unity project
    5. Write Firebase Unity WebGL library (Firebase don't provide package for the Unity's WebGL platform)

<h4> Smart Contract </h4>

This is a Smart Contract that manages MOL token and reward system.
Users can get daily rewards by MOL token.
The smart contract logs the user's last login time and sends 10 MOL tokens to the user's wallet every day.

- Implemented functions
    1. Create an ERC20 token(MOL)
    2. Save the user's last login time
    3. Check if the user can receive a daily reward
    4. Send 10 MOL tokens to the user's wallet if the user can receive

- Construct a building environment
    1. Install truffle
        > npm install -g truffle
    2. Compile the Smart Contract
        > truffle compile
    3. Run Truffle on local testnet(need to install Ganash)
        > truffle development
    4. Run Truffle on Mumbai testnet
        > truffle testnet
    5. Deploy Smart contract
        > migrate




