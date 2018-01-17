
# ToDo list items

This is a very simple .NET Core Terminal application for controlling the status
of the ToDo items in our testing database.

## Usage

`todo_list_items` can do two things:

1. List all the different ToDo items for all test users and the statuses of those ToDo items
2. Reset all ToDo items for all test users so that they're incomplete and have a "completed"

To list the different ToDo items, ensure that you are in this directory and run
the following command:

    dotnet run

This will compile and run this .NET Core Terminal application without any
arguments, causing `todo_list_items` to default to printing the status of all
ToDo items.

To reset all ToDo items, ensure you're in this directory and run the following
command:

    dotnet run -- reset

This will compile and run `todo_list_items` with an argument telling it to
reset the status of all ToDo items.

## Installing

Clone this repository, `cd` into it, and you'll be able to build/run the
project from there.

    git clone https://github.com/wsu-onering/todo_list_items.git
	cd todo_list_items
	# To run the project:
	dotnet run

