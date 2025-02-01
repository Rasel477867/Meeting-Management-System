
# Meeting  Management System

This Booking Management System is designed to handle various booking scenarios, including one-time bookings, daily, and weekly recurring bookings. It employs the Onion Architecture pattern to promote a clean and maintainable codebase.


## Architecture
The system is structured into several layers:

Core Layer (Domain Entities):

Contains the core business logic and domain entities.
Defines the Booking entity with properties such as BookingDate, StartTime, EndTime, RepetitionOption, and DaysToRepeatedOn.
Repository Layer:

Manages data access operations.
Provides methods to retrieve existing bookings and perform necessary checks.

Service Layer:

Contains business logic for booking operations.
Includes the IsBookingAvailableAsync method to check for booking availability based on repetition options.

Presentation Layer (MVC):

Handles user interactions and displays booking information.
Utilizes dynamic text and calendar views to show booking statuses.
## Features

Booking Availability Check:

Validates if a booking can be made based on the selected repetition option:

No Repeat: Checks for existing bookings at the exact date and time.

Daily Repeat: Checks for existing bookings on the same day of the week within the specified date range.

Weekly Repeat: Checks for existing bookings on the same days of the week within the specified date range.

