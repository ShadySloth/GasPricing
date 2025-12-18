Feature: Calculate gas price with invalid amount gas
    Calculate the price of gas when given an invalid amount of gas
    
    @myTag
    Scenario Outline: Calculate gas price with invalid amount
        Given you refuel with <amount> liters of gas
        When the application attempts to calculate the gas price
        Then an error message should be displayed indicating an invalid amount of gas
        
        Examples:
          | amount  |
          | -10     |
          | 0       |