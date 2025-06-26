Feature: Login screen
  As a product owner
  I need a login screen for my application
  So that users can access their accounts securely

  Scenario: Successful login with valid credentials
    Given the user is on the login page
    When the user enters a valid username and password
    And clicks the login button
    Then the user should be logged in to the application

  Scenario: Login fails with invalid credentials
    Given the user is on the login page
    When the user enters an invalid username or password
    And clicks the login button
    Then an error message should be displayed to the user

  Scenario: Validation error for empty fields
    Given the user is on the login page
    When the user leaves the username or password field empty
    And clicks the login button
    Then an error message should be displayed to the user
