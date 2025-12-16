Feature: Calculate gas price with wrong gas type
Calculate the price of gas when given an invalid gas type

    @myTag
    Scenario Outline: Calculate gas price with invalid gas type
        Given you refuel with <gasType> gas
        When the application tries to calculates the gas price
        Then an error message should be displayed indicating invalid gas type

        Examples:
          | gasType    |
          | "Electric" |
          | "Hydrogen" |
          | "Biofuel"  |
          | "Milk"     |