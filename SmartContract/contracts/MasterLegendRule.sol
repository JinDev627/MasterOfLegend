// SPDX-License-Identifier: MIT
pragma solidity ^0.8.13; 

import "@openzeppelin/contracts/token/ERC20/ERC20.sol"; 
import "@openzeppelin/contracts/access/Ownable.sol"; 

contract MasterLegendRule is Ownable{

    mapping (address => uint256) UserLastReceiveDailyRewardTime;
    ERC20 public MOLTokenContract;
    bool TokenContractValid;
    uint256 dailyRewardCount;

    event Received(address, uint);
    receive() external payable {
        emit Received(msg.sender, msg.value);
    }

    constructor() Ownable(){
        TokenContractValid = false;
        dailyRewardCount = (uint256)(1000000);
    }

    function setTokenContractAddress(address _tokenContractAddress) public onlyOwner {
        MOLTokenContract = ERC20(_tokenContractAddress);
        TokenContractValid = true;
    }
    function setDailyRewardCount(uint256 _dailyRewardCount) public onlyOwner {
        dailyRewardCount = _dailyRewardCount;
    }
    function getDailyRewardCount() public view returns(uint256) {
        return dailyRewardCount;
    }

    function getContractBalance() public view onlyOwner returns (uint256) {
        require(TokenContractValid == true , "Token Contract Address must be set");
        return MOLTokenContract.balanceOf(address(this));
    }

    function withdrawToken(uint256 amount) public onlyOwner returns (uint256)
    {
        require(TokenContractValid == true , "Token Contract Address must be set");
        if (MOLTokenContract.transfer(msg.sender, amount) == true){
            return amount;
        }
        return 0;
    }

    function requestDailyReward() public returns (uint256)
    {
        require(TokenContractValid == true , "Token Contract Address must be set");
        require(MOLTokenContract.balanceOf(address(this)) > dailyRewardCount , "Insufficient of balance");
        require(block.timestamp - UserLastReceiveDailyRewardTime[_msgSender()] > 1 days , "You should wait for more to receive the next reward.");
        
        MOLTokenContract.transfer(msg.sender, dailyRewardCount);
        UserLastReceiveDailyRewardTime[_msgSender()] = block.timestamp;

        return dailyRewardCount;
    }

    function getLastReceiveRewardTime(address userAddress) public view returns (uint256)
    {
        return UserLastReceiveDailyRewardTime[userAddress];
    }
    function getCurrentTimeStamp() public view returns (uint256)
    {
        return block.timestamp;
    }

    function CheckCanReceiveReward(address userAddress) public view returns (uint256)
    {
        if (block.timestamp - UserLastReceiveDailyRewardTime[userAddress] > 1 days)
        {
            return 0;
        }
        else
        {
            return block.timestamp - UserLastReceiveDailyRewardTime[userAddress];
        }
    }

}