using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class OpenSeaERC115Item
{
    public string contract;
    public string tokenId;
    public string uri;
    public string balance;
}
[Serializable]
public class OpenSeaInventory
{
    [SerializeField]
    public OpenSeaERC115Item[] items;
}
[Serializable]
public class ItemDescription
{
    public string image;
    public string name;
    public string description;
    public string external_link;
    public string animation_url;
    public bool isMasterOfLegends;

    public Texture LoadedItemTexture;
    public int balance;

    public ItemDescription(JObject itemDescObject)
    {
        isMasterOfLegends = false;
        name = (string)itemDescObject["name"];
        image = (string)itemDescObject["image"];
        description = (string)itemDescObject["description"];
        external_link = (string)itemDescObject["external_link"];
        animation_url = (string)itemDescObject["animation_url"];

        isMasterOfLegends = external_link.Contains("www.masteroflegends.com");
        LoadedItemTexture = null;
        balance = 0;
    }
}

public class RacingRuleWeb3
{
    private const string GameTokenContract = "0xF040B83b644426A3040Bd2bBb5813950a4c544E9";
    private const string GameRuleContractAddress = "0x2531065739f4e12932898f0baDF128F924227316";
    private const string GameRuleABI = "[{\"inputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"previousOwner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"OwnershipTransferred\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": false,\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"name\": \"Received\",\"type\": \"event\"},{\"inputs\": [],\"name\": \"MOLTokenContract\",\"outputs\": [{\"internalType\": \"contract ERC20\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [],\"name\": \"owner\",\"outputs\": [{\"internalType\": \"address\",\"name\": \"\",\"type\": \"address\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [],\"name\": \"renounceOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"newOwner\",\"type\": \"address\"}],\"name\": \"transferOwnership\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"stateMutability\": \"payable\",\"type\": \"receive\",\"payable\": true},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"_tokenContractAddress\",\"type\": \"address\"}],\"name\": \"setTokenContractAddress\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"_dailyRewardCount\",\"type\": \"uint256\"}],\"name\": \"setDailyRewardCount\",\"outputs\": [],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"getDailyRewardCount\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [],\"name\": \"getContractBalance\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"withdrawToken\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"requestDailyReward\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"userAddress\",\"type\": \"address\"}],\"name\": \"getLastReceiveRewardTime\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [],\"name\": \"getCurrentTimeStamp\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"userAddress\",\"type\": \"address\"}],\"name\": \"CheckCanReceiveReward\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\",\"constant\": true}]";
    private const string ERC20ABI = "[{\"inputs\": [{\"internalType\": \"string\",\"name\": \"name_\",\"type\": \"string\"},{\"internalType\": \"string\",\"name\": \"symbol_\",\"type\": \"string\"}],\"stateMutability\": \"nonpayable\",\"type\": \"constructor\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"value\",\"type\": \"uint256\"}],\"name\": \"Approval\",\"type\": \"event\"},{\"anonymous\": false,\"inputs\": [{\"indexed\": true,\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"indexed\": true,\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"indexed\": false,\"internalType\": \"uint256\",\"name\": \"value\",\"type\": \"uint256\"}],\"name\": \"Transfer\",\"type\": \"event\"},{\"inputs\": [],\"name\": \"name\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"symbol\",\"outputs\": [{\"internalType\": \"string\",\"name\": \"\",\"type\": \"string\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"decimals\",\"outputs\": [{\"internalType\": \"uint8\",\"name\": \"\",\"type\": \"uint8\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [],\"name\": \"totalSupply\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"account\",\"type\": \"address\"}],\"name\": \"balanceOf\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"transfer\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"owner\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"}],\"name\": \"allowance\",\"outputs\": [{\"internalType\": \"uint256\",\"name\": \"\",\"type\": \"uint256\"}],\"stateMutability\": \"view\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"approve\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"from\",\"type\": \"address\"},{\"internalType\": \"address\",\"name\": \"to\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"amount\",\"type\": \"uint256\"}],\"name\": \"transferFrom\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"addedValue\",\"type\": \"uint256\"}],\"name\": \"increaseAllowance\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"},{\"inputs\": [{\"internalType\": \"address\",\"name\": \"spender\",\"type\": \"address\"},{\"internalType\": \"uint256\",\"name\": \"subtractedValue\",\"type\": \"uint256\"}],\"name\": \"decreaseAllowance\",\"outputs\": [{\"internalType\": \"bool\",\"name\": \"\",\"type\": \"bool\"}],\"stateMutability\": \"nonpayable\",\"type\": \"function\"}]";

    private const string ERC1155ContractAddress = "0x2953399124F0cBB46d2CbACD8A89cF0599974963";


    private const string GET_REWARD_TIME_FUNCTION = "getLastReceiveRewardTime";
    private const string REQUEST_DAILY_REWARD_FUNCTION = "requestDailyReward";

    private const string chain = "polygon";
    //private const string network = "testnet";
    private const string network = "mainnet";

    async public static Task<string> getLastReceiveRewardTime(string address)
    {
        string[] obj = { address };
        string args = JsonConvert.SerializeObject(obj);
        Debug.Log("args = " + args);
        string response = await EVM.Call(chain, network, GameRuleContractAddress, GameRuleABI, GET_REWARD_TIME_FUNCTION, args);
        Debug.Log(response);
        return response;
    }

    async public static Task<string> requestDailyReward()
    {
        string args = "[]";
#if UNITY_EDITOR
        string response = await EVM.Call(chain, network, GameRuleContractAddress, GameRuleABI, REQUEST_DAILY_REWARD_FUNCTION, args);
#else
        string value = "0";
        string response = await Web3GL.SendContract(REQUEST_DAILY_REWARD_FUNCTION, GameRuleABI, GameRuleContractAddress, args, value);
#endif
        Debug.Log(response);
        return response;
    }

    async public static Task<string> ListAllErc1155()
    {
        string response = await EVM.AllErc1155(chain, network, GlobalController.Instance.USER_WALLET_ACCOUNT, ERC1155ContractAddress);
        Debug.Log(response);
        return response;
        
    }



}
