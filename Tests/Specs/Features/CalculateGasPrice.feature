Feature: CalculateGasPrice
Calculate the price of gas based on the amount, type and membership status

    @myTag
    Scenario Outline: Calculate gas price for different types and membership
        Given you refuel <amount> liters of <gasType> gas
        And the user has membership status set to <membership>
        When the application calculates the gas price
        Then the total price should be <totalPrice>

        Examples:
          | amount | gasType   | membership | totalPrice |
          | 20     | "Regular" | True       | 228.60     |
          | 20     | "Regular" | False      | 254.00     |
          | 20     | "Diesel"  | True       | 129.6      |
          | 20     | "Diesel"  | False      | 144.00     |
          | 50     | "Regular" | True       | 539.75     |
          | 50     | "Regular" | False      | 603.25     |
          | 50     | "Premium" | True       | 616.25     |
          | 50     | "Premium" | False      | 688.75     |
          | 100    | "Diesel"  | True       | 576.00     |
          | 100    | "Diesel"  | False      | 648.00     |
          | 100    | "Premium" | True       | 1160.00    |
          | 100    | "Premium" | False      | 1305.00    |