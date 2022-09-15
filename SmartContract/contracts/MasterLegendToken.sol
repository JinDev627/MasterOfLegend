// SPDX-License-Identifier: MIT
pragma solidity ^0.8.13; 

import "@openzeppelin/contracts/token/ERC20/ERC20.sol"; 
import "@openzeppelin/contracts/access/Ownable.sol"; 
//Master of Legends
contract MasterLegendToken is ERC20, Ownable
{ 
    uint public INITIAL_SUPPLY = 1000000000000000000;
    constructor(address gameRuleContract) ERC20("MasterLegendToken","MOL") Ownable(){ 
        _mint(gameRuleContract, INITIAL_SUPPLY); 
    }
    function decimals() public view virtual override returns (uint8) {
        return 5;
    }

}

