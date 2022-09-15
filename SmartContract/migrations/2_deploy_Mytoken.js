var gameToken = artifacts.require("MasterLegendToken");
var gameRule = artifacts.require("MasterLegendRule");
module.exports = async function (deployer) {
    await deployer.deploy(gameRule);
    let _gameContractInstance = await gameRule.deployed();
    await deployer.deploy(gameToken , _gameContractInstance.address);
    let _gameTokenInstance = await gameToken.deployed();
    let setTokenAddressResult = await _gameContractInstance.setTokenContractAddress(_gameTokenInstance.address);
    console.log(setTokenAddressResult);
};
  