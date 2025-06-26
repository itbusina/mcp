Feature: Dashboard
  As a developer
  I would like to have a dashboard for my application
  So that I can monitor and manage key information easily

  Scenario: Dashboard displays key information after login
    Given the user is logged in
    When the user navigates to the dashboard page
    Then the dashboard should display key information relevant to the user

  Scenario: Accessing dashboard without login
    Given the user is not logged in
    When the user tries to access the dashboard page
    Then the user should be redirected to the login page

  Scenario: Dashboard shows error if data cannot be loaded
    Given the user is logged in
    When the dashboard fails to load data
    Then an error message should be displayed to the user
