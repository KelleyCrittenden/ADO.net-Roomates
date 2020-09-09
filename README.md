# ADO.net-Roomates

Sometimes money is tight. Sometimes when money is tight, it becomes an unavoidable truth that we must live with other people. We call these people Roommates.

Your task is to build a command line application to manage a house full of roommates. You should persist data in a SQL Server database.

## Exercise
Implement the RoommateRepository class to include the following methods

public List<Roommate> GetAll()
Roommate objects should have a null value for their Room property
public Roommate GetById(int id)
public List<Roommate> GetRoommatesByRoomId(int roomId)
Roommate objects should have a Room property
public void Insert(Roommate roommate)
public void Update(Roommate roommate)
public void Delete(int id)
Update the Program.Main method to print a report of all roommates and their rooms
