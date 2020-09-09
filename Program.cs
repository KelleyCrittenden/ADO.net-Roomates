using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;


namespace Roommates
{
    class Program
    {
        /// <summary>
        ///  This is the address of the database.
        ///  We define it here as a constant since it will never change.
        /// </summary>
        /// address of our database, 
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Rooms:");
            Console.WriteLine();

            //returning list of rooms
            List<Room> allRooms = roomRepo.GetAll();
            //looping over room list and printing them all out
            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }


            //calling getbyID method to print out the results
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Room with Id 1");

            Room singleRoom = roomRepo.GetById(1);

            Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            Room bathroom = new Room
            {
                Name = "Bathroom",
                MaxOccupancy = 1
            };

            roomRepo.Insert(bathroom);

            Console.WriteLine("-------------------------------");
            Console.WriteLine($"Added the new Room with id {bathroom.Id}");

            bathroom.MaxOccupancy = 3;
            roomRepo.Update(bathroom);

            Room bathroomFromDb = roomRepo.GetById(bathroom.Id);
                Console.WriteLine($"{bathroomFromDb.Id} {bathroomFromDb.Name} {bathroomFromDb.MaxOccupancy}");
            Console.WriteLine("-------------------------------");

            roomRepo.Delete(bathroom.Id);

            allRooms = roomRepo.GetAll();
            foreach (Room room in allRooms)
            {
                Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            }
            ////////////////Roommate///////////////////////
           
            ///////////Getting All Roommates w/o Room//////
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Roommates:");
            Console.WriteLine();

            List<Roommate> allRoommates = roommateRepo.GetAll();
        
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} {roommate.FirstName} {roommate.LastName} {roommate.RentPortion} {roommate.MoveInDate}");
            }
            ///////////Getting One Single Roommate by Id //////
           
            Console.WriteLine("----------------------------");
            Console.WriteLine("Getting Roomate with Id 1");

            Roommate singleRoommate = roommateRepo.GetById(1);

            Console.WriteLine($"{singleRoommate.Id} {singleRoommate.FirstName} {singleRoommate.LastName} {singleRoommate.RentPortion} {singleRoommate.MoveInDate}");

            ///////////Getting All Rooms with Room Object Attatched //////
            RoommateRepository roommateRoomRepo = new RoommateRepository(CONNECTION_STRING);

            Console.WriteLine("Getting All Roommates with Room:");
            Console.WriteLine();

            List<Roommate> allRoommateswithRoom = roommateRoomRepo.GetAllWithRoom(2);

            foreach (Roommate roommateWithRoom in allRoommateswithRoom)
            {
                Console.WriteLine($"{roommateWithRoom.Id} {roommateWithRoom.FirstName} {roommateWithRoom.LastName} {roommateWithRoom.RentPortion} {roommateWithRoom.MoveInDate} {roommateWithRoom.Room.Name} {roommateWithRoom.Room.MaxOccupancy}");
            }


            ///////////Adding in New Roommate //////
            Roommate newRoommate = new Roommate
            {
                FirstName = "Wendy",
                LastName = "Jones",
                MoveInDate = DateTime.Now.AddDays(-1),
                RoomId = 1,
                RentPortion = 10
            };
            roommateRepo.Insert(newRoommate);


            ///////////Updating Roommate //////
            newRoommate.LastName = "Smith";

            roommateRepo.Update(newRoommate);

            allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} First Name : {roommate.FirstName} Last Name: {roommate.LastName} Rent Portion: {roommate.RentPortion} Date Moved In : {roommate.MoveInDate}");
            }
            Console.WriteLine("-----------------------------------------");

            ///////////Deleting Roommate ///////////
            roommateRepo.Delete(newRoommate.Id);

            allRoommates = roommateRepo.GetAll();
            foreach (Roommate roommate in allRoommates)
            {
                Console.WriteLine($"{roommate.Id} First Name : {roommate.FirstName} Last Name: {roommate.LastName} Rent Portion: {roommate.RentPortion} Date Moved In : {roommate.MoveInDate}");
            }

        }

    }
}
