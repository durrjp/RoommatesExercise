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
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            //Console.WriteLine("Getting All Rooms:");
            //Console.WriteLine();

            //List<Room> allRooms = roomRepo.GetAll();

            //foreach (Room room in allRooms)
            //{
            //    Console.WriteLine($"{room.Id} {room.Name} {room.MaxOccupancy}");
            //}

            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Getting Room with Id 1");

            //Room singleRoom = roomRepo.GetById(1);

            //Console.WriteLine($"{singleRoom.Id} {singleRoom.Name} {singleRoom.MaxOccupancy}");

            //Room bathroom = new Room
            //{
            //    Name = "Bathroom",
            //    MaxOccupancy = 1
            //};

            //roomRepo.Insert(bathroom);

            //Console.WriteLine("-------------------------------");
            //Console.WriteLine($"Added the new Room with id {bathroom.Id}");

            //Room washroom = new Room
            //{
            //    Id = 7,
            //    Name = "Washroom",
            //    MaxOccupancy = 2
            //};

            //roomRepo.Update(washroom);

            while (true)
            {
                Console.WriteLine();
                int selection = Menu();
                switch(selection)
                {
                    case 1:
                        List<Roommate> allRoommates = roommateRepo.GetAll();

                        foreach (Roommate roommate in allRoommates)
                        {
                            Console.WriteLine(@$"{roommate.Firstname} {roommate.Lastname} {roommate.RentPortion}
Living in the {roommate.Room.Name}");
                        }
                        break;
                    case 2:
                        List<Roommate> roomies = roommateRepo.GetAllWithRoom(3);
                        foreach (Roommate roommate in roomies)
                        {
                            Console.WriteLine($"{roommate.Firstname}");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Roommate First Name:");
                        string FirstName = Console.ReadLine();
                        Console.WriteLine("Roommate Last Name:");
                        string LastName = Console.ReadLine();
                        Console.WriteLine("Rent Portion:");
                        string RentString = Console.ReadLine();
                        int RentInt = Int32.Parse(RentString);
                        DateTime todaysDate = DateTime.Now;
                        Console.WriteLine("Room Id:");
                        string RoomIdString = Console.ReadLine();
                        int RoomIdInt = Int32.Parse(RoomIdString);
                        Room singleRoom = roomRepo.GetById(RoomIdInt);
                        Roommate newRoomy = new Roommate()
                        {
                            Firstname = FirstName,
                            Lastname = LastName,
                            RentPortion = RentInt,
                            MovedInDate = todaysDate,
                            Room = singleRoom
                        };
                        roommateRepo.Insert(newRoomy);
                        break;
                    case 4:
                        Console.WriteLine("Enter Roommate Id to Update:");
                        string roomyString = Console.ReadLine();
                        int roomyInt = Int32.Parse(roomyString);
                        Roommate roomyToUpdate = roommateRepo.GetById(roomyInt);

                        Console.WriteLine($"Enter updated roomy First Name from {roomyToUpdate.Firstname}:");
                        string firstName = Console.ReadLine();

                        Console.WriteLine($"Enter updated roomy Last Name from {roomyToUpdate.Lastname}");
                        string lastName = Console.ReadLine();

                        Console.WriteLine($"Update Rent Portion from {roomyToUpdate.RentPortion}");
                        string RentStringed = Console.ReadLine();
                        int RentInted = Int32.Parse(RentStringed);

                        Console.WriteLine($"Enter updated room Id from {roomyToUpdate.Room.Id}");
                        string RoomyIdString = Console.ReadLine();
                        int RoomyIdInt = Int32.Parse(RoomyIdString);
                        Room updateSingleRoom = roomRepo.GetById(RoomyIdInt);
                        Roommate updatedRoomy = new Roommate
                        {
                            Firstname = firstName,
                            Lastname = lastName,
                            RentPortion = RentInted,
                            MovedInDate = roomyToUpdate.MovedInDate,
                            Room = updateSingleRoom,
                            Id = roomyToUpdate.Id
                        };
                        roommateRepo.Update(updatedRoomy);

                        break;
                    case 5:
                        Console.WriteLine("Enter Roommate id to kick from the house:");
                        string stringOfRoomyKick = Console.ReadLine();
                        int intOfRoomyKick = Int32.Parse(stringOfRoomyKick);
                        roommateRepo.Delete(intOfRoomyKick);
                        break;
                    case 0:
                        Console.WriteLine("Goodbye");
                        return;
                    default:
                        throw new Exception("Something went wrong...invalid selection");
                }

            }

            static int Menu()
            {
                int selection = -1;

                while (selection < 0 || selection > 5)
                {
                    Console.WriteLine("Roommates Menu");
                    Console.WriteLine(" 1) List all roommates");
                    Console.WriteLine(" 2) Get roommates living in room:");
                    Console.WriteLine(" 3) Add new roommate");
                    Console.WriteLine(" 4) Update roommate info");
                    Console.WriteLine(" 5) Remove roommate");
                    Console.WriteLine(" 0) Exit");

                    Console.Write("> ");
                    string choice = Console.ReadLine();
                    try
                    {
                        selection = int.Parse(choice);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid Selection. Please try again.");
                    }
                    Console.WriteLine();
                }

                return selection;
            }
        }
    }
}
