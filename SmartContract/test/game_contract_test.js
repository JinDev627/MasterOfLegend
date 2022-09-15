const gameContract = artifacts.require("MasterLegendRule");
const gameToken = artifacts.require("MasterLegendToken");

contract("Check token balances", async accounts => {
    it("should put 10000000000 token in the first account", async () => {
        const instance = await gameContract.deployed();
        const balance = await instance.getContractBalance();
        console.log("Contract Balance:", balance.toNumber());
        assert.equal(balance.toNumber(), 1000000000000000);
    });

    it("request daily reward - first ", async () => {
        const instance = await gameContract.deployed();
        const gameTokenInstace = await gameToken.deployed();

        var canReceiveReward0 = await instance.CheckCanReceiveReward(accounts[0]);
        var canReceiveReward1 = await instance.CheckCanReceiveReward(accounts[1]);
        assert.equal(canReceiveReward0, 0);
        assert.equal(canReceiveReward1, 0);
        var dailyReward = await instance.requestDailyReward();
        var accountBalance = await gameTokenInstace.balanceOf(accounts[0])
        console.log("Daily Rward:", dailyReward , accountBalance.toNumber());
        //assert.equal(dailyReward.toNumber(), accountBalance.toNumber());
    });

    it("request daily reward - second", async () => {
        const instance = await gameContract.deployed();
        var canReceiveReward0 = await instance.CheckCanReceiveReward(accounts[0]);
        var canReceiveReward1 = await instance.CheckCanReceiveReward(accounts[1]);
        console.log("Remain time to receive next reward:" , canReceiveReward0.toNumber());
        assert.equal(canReceiveReward1, 0);
        //var dailyReward = await instance.requestDailyReward();
        //assert.equal(dailyReward, 0);
    });

    
});