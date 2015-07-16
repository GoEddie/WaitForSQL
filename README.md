# WaitForSQL

This is a simple app that does just one thing, it tries to create connections to a SQL Server database and returns either after a timeout or when a connection has been made.

## Wat?

In cases like building continuous integration processes for SQL Server you sometimes want a new instance or database but can't always be sure when you can connect especially on slower build machines where multiple concurrent builds might make start times inconsistent.

