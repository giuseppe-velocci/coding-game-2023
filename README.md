# API Documentation for Coding Game 2023: Order Management 

This document provides details about the endpoints for managing orders, adding products, setting payment methods, and verifying results.

## Endpoints

### Create an Order

- **HTTP Method**: POST
- **Endpoint**: `/order`
- **Description**: Create a new order. returns the Key of the created Order. Must be passed to other POST endpoints.

### Add a Product to an Order

- **HTTP Method**: POST
- **Endpoint**: `/order/add-product/{id}`
- **Description**: Add a product (drink) to an existing order. Calling this endpoint multiple times for the same product will result in an override. It cannot remove added products. <br>*Important*: products can be added as long as a payment has not been added.
- **Query Parameters**: id: Order id, returned by `/order` endpoint
- **HTTP Request Body**:
  ```json
  {
      "Name": "<product name>",
      "Quantity": <product quantity>
  }
  ```

### Add a Payment method to an Order

- **HTTP Method**: POST
- **Endpoint**: `/order/add-payment/{id}`
- **Description**: Add a payment method to an existing order. <br>*Important*: a payment can be added only if an order has at least one product. After a payment is set, no more products can be added to that order.
- **Query Parameters**: id: Order id, returned by `/order` endpoint
- **HTTP Request Body**:
  ```json
  {
      "Name": "<payment method name>"
  }
  ```

### Get Order Details

- **HTTP Method**: GET
- **Endpoint**: `/order/{id}`
- **Description**: Retrieve details of a specific order.
- **Query Parameters**: id: Order id, returned by `/order` endpoint

### Get the List of Products

- **HTTP Method**: GET
- **Endpoint**: `/drinks`
- **Description**: Retrieve a list of available products (drinks). When adding a product full name with must be passed respecting letter casing.

### Get the List of Payment Methods

- **HTTP Method**: GET
- **Endpoint**: `/payments`
- **Description**: Retrieve a list of available payment methods. When adding a payment method full name with must be passed respecting letter casing.