﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace StreamingDatasetsDemo.Models {

  public class Contribution {
    public int ContributionID { get; set; }
    public string Contributor { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zipcode { get; set; }
    public string Gender { get; set; }
    public DateTime Time { get; set; }
    public string TimeWindow { get; set; }
    public decimal Amount { get; set; }
  }

  class ContributionSet {
    public Contribution[] rows { get; set; }
  }

  #region "Classes for generating sample data for contribution demo"

  public class LocationData {
    public string City { get; set; }
    public string State { get; set; }
    public int Zipcode { get; set; }
  }

  public class ContributionData {
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public int Zipcode { get; set; }
    public string Gender { get; set; }
    public DateTime Time { get; set; }
    public string TimeWindow { get; set; }
    public decimal Amount { get; set; }
  }

  #endregion

  public class DataFactory {

    private static Random RandomNumberFactory = new Random(2016);
    private static int contributionCount = 0;
    private static int contributionListsCount = 0;
    private static int ContributionGrowthPhase = 1;

    public static void SetContributionGrowthPhase(int value) {
      ContributionGrowthPhase = value; 
    }

    public static ContributionData GetNextContribution() {
      contributionCount += 1;
      LocationData contributionLocation = GetNextContributionLocation();

      int contributionId = contributionCount;
      string City = contributionLocation.City;
      string State = contributionLocation.State;
      int Zipcode = contributionLocation.Zipcode;

      string Gender = "Female";

      switch (ContributionGrowthPhase) {
        case 1:
          if (RandomNumberFactory.Next(1, 100) > 36) { Gender = "Male"; }
          break;
        case 2:
          if (RandomNumberFactory.Next(1, 100) > 72) { Gender = "Male"; }
          break;
        case 3:
          if (RandomNumberFactory.Next(1, 100) > 80) { Gender = "Male"; }
          break;
        case 4:
          if (RandomNumberFactory.Next(1, 100) > 72) { Gender = "Male"; }
          break;
        case 5:
          if (RandomNumberFactory.Next(1, 100) > 62) { Gender = "Male"; }
          break;
        case 6:
          if (RandomNumberFactory.Next(1, 100) > 45) { Gender = "Male"; }
          break;
        case 7:
          if (RandomNumberFactory.Next(1, 100) > 48) { Gender = "Male"; }
          break;
        case 8:
          if (RandomNumberFactory.Next(1, 100) > 40) { Gender = "Male"; }
          break;
        case 9:
          if (RandomNumberFactory.Next(1, 100) > 50) { Gender = "Male"; }
          break;
        case 10:
          if (RandomNumberFactory.Next(1, 100) > 58) { Gender = "Male"; }
          break;
      }

      string FirstName = "";
      if (Gender.Equals("F")) {
        FirstName = GetNextFemaleFirstName();
      }
      else {
        FirstName = GetNextMaleFirstName();
      }

      string LastName = GetNextLastName();

      string TimeValue = DateTime.Now.ToString("h:mm") + ":" + ((DateTime.Now.Second / 15) * 15).ToString("00");

      ContributionData newContribution = new ContributionData {
        ID = contributionId,
        FirstName = FirstName,
        LastName = LastName,
        City = City,
        State = State,
        Zipcode = Zipcode,
        Gender = Gender,
        Time = DateTime.Now.AddHours(-4),
        TimeWindow = TimeValue,
        Amount = GetNextContributionAmount(Zipcode)
      };
      return newContribution;
    }

    #region " static fields with arrays of field values"

    private static LocationData[] ContributionLocations1 = new LocationData[]{
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Davis Island", State="FL", Zipcode=33606 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 }
    };

    private static LocationData[] ContributionLocations2 = new LocationData[]{
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="Davis Island", State="FL", Zipcode=33606 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621},
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
    };

    private static LocationData[] ContributionLocations3 = new LocationData[]{
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Davis Island", State="FL", Zipcode=33606 }
    };

    private static LocationData[] ContributionLocations4 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 }
    };

    private static LocationData[] ContributionLocations5 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 }
    };

    private static LocationData[] ContributionLocations6 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Davis Island", State="FL", Zipcode=33606 }
    };

    private static LocationData[] ContributionLocations7 = new LocationData[] {
     new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Town N Country", State="FL", Zipcode=33615 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33634 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33610 },
      new LocationData{ City="Clair Mel City", State="FL", Zipcode=33619 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33607 },
      new LocationData{ City="Palma Ceia", State="FL", Zipcode=33629 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33767 }
    };

    private static LocationData[] ContributionLocations8 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Town N Country", State="FL", Zipcode=33615 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33634 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33610 },
      new LocationData{ City="Clair Mel City", State="FL", Zipcode=33619 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33607 },
      new LocationData{ City="Palma Ceia", State="FL", Zipcode=33629 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33767 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33764 },
      new LocationData{ City="Indian Rocks Beach", State="FL", Zipcode=33786 },
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33709 }
    };

    private static LocationData[] ContributionLocations9 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Town N Country", State="FL", Zipcode=33615 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33634 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33610 },
      new LocationData{ City="Clair Mel City", State="FL", Zipcode=33619 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33607 },
      new LocationData{ City="Palma Ceia", State="FL", Zipcode=33629 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33767 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33764 },
      new LocationData{ City="Indian Rocks Beach", State="FL", Zipcode=33786 },
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33709 },
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Belleair Bluffs", State="FL", Zipcode=33770 }
    };

    private static LocationData[] ContributionLocations10 = new LocationData[] {
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Town N Country", State="FL", Zipcode=33615 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33634 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33610 },
      new LocationData{ City="Clair Mel City", State="FL", Zipcode=33619 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33607 },
      new LocationData{ City="Palma Ceia", State="FL", Zipcode=33629 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33767 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33764 },
      new LocationData{ City="Indian Rocks Beach", State="FL", Zipcode=33786 },
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33709 },
      new LocationData{ City="Odessa", State="FL", Zipcode=33556 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33558 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33548 },
      new LocationData{ City="Lutz", State="FL", Zipcode=33559 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34683 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34684 },
      new LocationData{ City="Palm Harbor", State="FL", Zipcode=34685 },
      new LocationData{ City="Westchase", State="FL", Zipcode=33626 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33625 },
      new LocationData{ City="Carrollwood", State="FL", Zipcode=33618 },
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34689},
      new LocationData{ City="Tarpon Springs", State="FL", Zipcode=34688 },
      new LocationData{ City="Lake Magdalene", State="FL", Zipcode=33613 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33617 },
      new LocationData{ City="Tampa Palms", State="FL", Zipcode=33647 },
      new LocationData{ City="Thonotosassa", State="FL", Zipcode=33592 },
      new LocationData{ City="Belleair Bluffs", State="FL", Zipcode=33770 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33634 },
      new LocationData{ City="Temple Terrace", State="FL", Zipcode=33610 },
      new LocationData{ City="Clair Mel City", State="FL", Zipcode=33619 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33602 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33607 },
      new LocationData{ City="Palma Ceia", State="FL", Zipcode=33629 },
      new LocationData{ City="Tampa", State="FL", Zipcode=33616 },
      new LocationData{ City="Macdill Air Force Base, Port Tampa", State="FL", Zipcode=33621 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33767 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33755 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33765 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33759 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33764 },
      new LocationData{ City="Indian Rocks Beach", State="FL", Zipcode=33786 },
      new LocationData{ City="Belleair Bluffs", State="FL", Zipcode=33770 },
      new LocationData{ City="Largo", State="FL", Zipcode=33771 },
      new LocationData{ City="Clearwater", State="FL", Zipcode=33760 },
      new LocationData{ City="Largo", State="FL", Zipcode=33773 },
      new LocationData{ City="Largo", State="FL", Zipcode=33774 },
      new LocationData{ City="Pinellas Park", State="FL", Zipcode=33782 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33709 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33713 },
      new LocationData{ City="St Petersburg", State="FL", Zipcode=33713 },
      new LocationData{ City="New Port Richey", State="FL", Zipcode=34625 },
      new LocationData{ City="Wesley Chappel", State="FL", Zipcode=33544 },
      new LocationData{ City="Dunedin", State="FL", Zipcode=34698 },
      new LocationData{ City="Apollo Beach", State="FL", Zipcode=33572 },
      new LocationData{ City="RiverView", State="FL", Zipcode=33569 }
    };

    private static string[] FemaleFirstNames = new string[] {
        "Abby", "Abigail", "Ada", "Addie", "Adela", "Adele", "Adeline", "Adrian", "Adriana", "Adrienne", "Agnes", "Aida", "Aileen", "Aimee", "Aisha", "Alana",
        "Alba", "Alberta", "Alejandra", "Alexandra", "Alexandria", "Alexis", "Alfreda", "Alice", "Alicia", "Aline", "Alisa", "Alisha", "Alison", "Alissa", "Allie", "Allison",
        "Allyson", "Alma", "Alta", "Althea", "Alyce", "Alyson", "Alyssa", "Amalia", "Amanda", "Amber", "Amelia", "Amie", "Amparo", "Amy", "Ana", "Anastasia",
        "Andrea", "Angel", "Angela", "Angelia", "Angelica", "Angelina", "Angeline", "Angelique", "Angelita", "Angie", "Anita", "Ann", "Anna", "Annabelle", "Anne", "Annette",
        "Annie", "Annmarie", "Antoinette", "Antonia", "April", "Araceli", "Arlene", "Arline", "Ashlee", "Ashley", "Audra", "Audrey", "Augusta", "Aurelia", "Aurora", "Autumn",
        "Ava", "Avis", "Barbara", "Barbra", "Beatrice", "Beatriz", "Becky", "Belinda", "Benita", "Bernadette", "Bernadine", "Bernice", "Berta", "Bertha", "Bertie", "Beryl",
        "Bessie", "Beth", "Bethany", "Betsy", "Bette", "Bettie", "Betty", "Bettye", "Beulah", "Beverley", "Beverly", "Bianca", "Billie", "Blanca", "Blanche", "Bobbi",
        "Bobbie", "Bonita", "Bonnie", "Brandi", "Brandie", "Brandy", "Brenda", "Briana", "Brianna", "Bridget", "Bridgett", "Bridgette", "Brigitte", "Britney", "Brittany", "Brittney",
        "Brooke", "Caitlin", "Callie", "Camille", "Candace", "Candice", "Candy", "Cara", "Carey", "Carissa", "Carla", "Carlene", "Carly", "Carmela", "Carmella", "Carmen",
        "Carol", "Carole", "Carolina", "Caroline", "Carolyn", "Carrie", "Casandra", "Casey", "Cassandra", "Cassie", "Catalina", "Catherine", "Cathleen", "Cathryn", "Cathy", "Cecelia",
        "Cecile", "Cecilia", "Celeste", "Celia", "Celina", "Chandra", "Charity", "Charlene", "Charlotte", "Charmaine", "Chasity", "Chelsea", "Cheri", "Cherie", "Cherry", "Cheryl",
        "Chris", "Christa", "Christi", "Christian", "Christie", "Christina", "Christine", "Christy", "Chrystal", "Cindy", "Claire", "Clara", "Clare", "Clarice", "Clarissa", "Claudette",
        "Claudia", "Claudine", "Cleo", "Coleen", "Colette", "Colleen", "Concepcion", "Concetta", "Connie", "Constance", "Consuelo", "Cora", "Corina", "Corine", "Corinne", "Cornelia",
        "Corrine", "Courtney", "Cristina", "Crystal", "Cynthia", "Daisy", "Dale", "Dana", "Danielle", "Daphne", "Darcy", "Darla", "Darlene", "Dawn", "Deana", "Deann",
        "Deanna", "Deanne", "Debbie", "Debora", "Deborah", "Debra", "Dee", "Deena", "Deidre", "Deirdre", "Delia", "Della", "Delores", "Deloris", "Dena", "Denise",
        "Desiree", "Diana", "Diane", "Diann", "Dianna", "Dianne", "Dina", "Dionne", "Dixie", "Dollie", "Dolly", "Dolores", "Dominique", "Dona", "Donna", "Dora",
        "Doreen", "Doris", "Dorothea", "Dorothy", "Dorthy", "Earlene", "Earline", "Earnestine", "Ebony", "Eddie", "Edith", "Edna", "Edwina", "Effie", "Eileen", "Elaine",
        "Elba", "Eleanor", "Elena", "Elinor", "Elisa", "Elisabeth", "Elise", "Eliza", "Elizabeth", "Ella", "Ellen", "Elma", "Elnora", "Eloise", "Elsa", "Elsie",
        "Elva", "Elvia", "Elvira", "Emilia", "Emily", "Emma", "Enid", "Erica", "Ericka", "Erika", "Erin", "Erma", "Erna", "Ernestine", "Esmeralda", "Esperanza",
        "Essie", "Estela", "Estella", "Estelle", "Ester", "Esther", "Ethel", "Etta", "Eugenia", "Eula", "Eunice", "Eva", "Evangelina", "Evangeline", "Eve", "Evelyn",
        "Faith", "Fannie", "Fanny", "Fay", "Faye", "Felecia", "Felicia", "Fern", "Flora", "Florence", "Florine", "Flossie", "Fran", "Frances", "Francesca", "Francine",
        "Francis", "Francisca", "Frankie", "Freda", "Freida", "Frieda", "Gabriela", "Gabrielle", "Gail", "Gale", "Gay", "Gayle", "Gena", "Geneva", "Genevieve", "Georgette",
        "Georgia", "Georgina", "Geraldine", "Gertrude", "Gilda", "Gina", "Ginger", "Gladys", "Glenda", "Glenna", "Gloria", "Goldie", "Grace", "Gracie", "Graciela", "Greta",
        "Gretchen", "Guadalupe", "Gwen", "Gwendolyn", "Haley", "Hallie", "Hannah", "Harriet", "Harriett", "Hattie", "Hazel", "Heather", "Heidi", "Helen", "Helena", "Helene",
        "Helga", "Henrietta", "Herminia", "Hester", "Hilary", "Hilda", "Hillary", "Hollie", "Holly", "Hope", "Ida", "Ila", "Ilene", "Imelda", "Imogene", "Ina",
        "Ines", "Inez", "Ingrid", "Irene", "Iris", "Irma", "Isabel", "Isabella", "Isabelle", "Iva", "Ivy", "Jackie", "Jacklyn", "Jaclyn", "Jacqueline", "Jacquelyn",
        "Jaime", "James", "Jami", "Jamie", "Jan", "Jana", "Jane", "Janell", "Janelle", "Janet", "Janette", "Janice", "Janie", "Janine", "Janis", "Janna",
        "Jannie", "Jasmine", "Jayne", "Jean", "Jeanette", "Jeanie", "Jeanine", "Jeanne", "Jeannette", "Jeannie", "Jeannine", "Jenifer", "Jenna", "Jennie", "Jennifer", "Jenny",
        "Jeri", "Jerri", "Jerry", "Jessica", "Jessie", "Jewel", "Jewell", "Jill", "Jillian", "Jimmie", "Jo", "Joan", "Joann", "Joanna", "Joanne", "Jocelyn",
        "Jodi", "Jodie", "Jody", "Johanna", "John", "Johnnie", "Jolene", "Joni", "Jordan", "Josefa", "Josefina", "Josephine", "Josie", "Joy", "Joyce", "Juana",
        "Juanita", "Judith", "Judy", "Julia", "Juliana", "Julianne", "Julie", "Juliet", "Juliette", "June", "Justine", "Kaitlin", "Kara", "Karen", "Kari", "Karin",
        "Karina", "Karla", "Karyn", "Kasey", "Kate", "Katelyn", "Katharine", "Katherine", "Katheryn", "Kathie", "Kathleen", "Kathrine", "Kathryn", "Kathy", "Katie", "Katina",
        "Katrina", "Katy", "Kay", "Kaye", "Kayla", "Keisha", "Kelley", "Kelli", "Kellie", "Kelly", "Kelsey", "Kendra", "Kenya", "Keri", "Kerri", "Kerry",
        "Kim", "Kimberley", "Kimberly", "Kirsten", "Kitty", "Kris", "Krista", "Kristen", "Kristi", "Kristie", "Kristin", "Kristina", "Kristine", "Kristy", "Krystal", "Lacey",
        "Lacy", "Ladonna", "Lakeisha", "Lakisha", "Lana", "Lara", "Latasha", "Latisha", "Latonya", "Latoya", "Laura", "Laurel", "Lauren", "Lauri", "Laurie", "Laverne",
        "Lavonne", "Lawanda", "Lea", "Leah", "Leann", "Leanna", "Leanne", "Lee", "Leigh", "Leila", "Lela", "Lelia", "Lena", "Lenora", "Lenore", "Leola",
        "Leona", "Leonor", "Lesa", "Lesley", "Leslie", "Lessie", "Leta", "Letha", "Leticia", "Letitia", "Lidia", "Lila", "Lilia", "Lilian", "Liliana", "Lillian",
        "Lillie", "Lilly", "Lily", "Lina", "Linda", "Lindsay", "Lindsey", "Lisa", "Liz", "Liza", "Lizzie", "Lois", "Lola", "Lolita", "Lora", "Loraine",
        "Lorena", "Lorene", "Loretta", "Lori", "Lorie", "Lorna", "Lorraine", "Lorrie", "Lottie", "Lou", "Louella", "Louisa", "Louise", "Lourdes", "Luann", "Lucia",
        "Lucile", "Lucille", "Lucinda", "Lucy", "Luella", "Luisa", "Lula", "Lupe", "Luz", "Lydia", "Lynda", "Lynette", "Lynn", "Lynne", "Lynnette", "Mabel",
        "Mable", "Madeleine", "Madeline", "Madelyn", "Madge", "Mae", "Magdalena", "Maggie", "Mai", "Malinda", "Mallory", "Mamie", "Mandy", "Manuela", "Mara", "Marcella",
        "Marci", "Marcia", "Marcie", "Marcy", "Margaret", "Margarita", "Margery", "Margie", "Margo", "Margret", "Marguerite", "Mari", "Maria", "Marian", "Mariana", "Marianne",
        "Maribel", "Maricela", "Marie", "Marietta", "Marilyn", "Marina", "Marion", "Marisa", "Marisol", "Marissa", "Maritza", "Marjorie", "Marla", "Marlene", "Marquita", "Marsha",
        "Marta", "Martha", "Martina", "Marva", "Mary", "Maryann", "Maryanne", "Maryellen", "Marylou", "Matilda", "Mattie", "Maude", "Maura", "Maureen", "Mavis", "Maxine",
        "May", "Mayra", "Meagan", "Megan", "Meghan", "Melanie", "Melba", "Melinda", "Melisa", "Melissa", "Melody", "Melva", "Mercedes", "Meredith", "Merle", "Mia",
        "Michael", "Michele", "Michelle", "Milagros", "Mildred", "Millicent", "Millie", "Mindy", "Minerva", "Minnie", "Miranda", "Miriam", "Misty", "Mitzi", "Mollie", "Molly",
        "Mona", "Monica", "Monique", "Morgan", "Muriel", "Myra", "Myrna", "Myrtle", "Nadia", "Nadine", "Nancy", "Nanette", "Nannie", "Naomi", "Natalia", "Natalie",
        "Natasha", "Nelda", "Nell", "Nellie", "Nettie", "Neva", "Nichole", "Nicole", "Nikki", "Nina", "Nita", "Noelle", "Noemi", "Nola", "Nona", "Nora",
        "Noreen", "Norma", "Odessa", "Ofelia", "Ola", "Olga", "Olive", "Olivia", "Ollie", "Opal", "Ophelia", "Ora", "Paige", "Pam", "Pamela", "Pansy",
        "Pat", "Patrica", "Patrice", "Patricia", "Patsy", "Patti", "Patty", "Paula", "Paulette", "Pauline", "Pearl", "Pearlie", "Peggy", "Penelope", "Penny", "Petra",
        "Phoebe", "Phyllis", "Polly", "Priscilla", "Queen", "Rachael", "Rachel", "Rachelle", "Rae", "Ramona", "Randi", "Raquel", "Reba", "Rebecca", "Rebekah", "Regina",
        "Rena", "Rene", "Renee", "Reva", "Reyna", "Rhea", "Rhoda", "Rhonda", "Rita", "Robbie", "Robert", "Roberta", "Robin", "Robyn", "Rochelle", "Ronda",
        "Rosa", "Rosalie", "Rosalind", "Rosalinda", "Rosalyn", "Rosanna", "Rosanne", "Rosario", "Rose", "Roseann", "Rosella", "Rosemarie", "Rosemary", "Rosetta", "Rosie", "Roslyn",
        "Rowena", "Roxanne", "Roxie", "Ruby", "Ruth", "Ruthie", "Sabrina", "Sadie", "Sallie", "Sally", "Samantha", "Sandra", "Sandy", "Sara", "Sarah", "Sasha",
        "Saundra", "Savannah", "Selena", "Selma", "Serena", "Shana", "Shanna", "Shannon", "Shari", "Sharlene", "Sharon", "Sharron", "Shauna", "Shawn", "Shawna", "Sheena",
        "Sheila", "Shelby", "Shelia", "Shelley", "Shelly", "Sheree", "Sheri", "Sherri", "Sherrie", "Sherry", "Sheryl", "Shirley", "Silvia", "Simone", "Socorro", "Sofia",
        "Sondra", "Sonia", "Sonja", "Sonya", "Sophia", "Sophie", "Stacey", "Staci", "Stacie", "Stacy", "Stefanie", "Stella", "Stephanie", "Sue", "Summer", "Susan",
        "Susana", "Susanna", "Susanne", "Susie", "Suzanne", "Suzette", "Sybil", "Sylvia", "Tabatha", "Tabitha", "Tamara", "Tameka", "Tamera", "Tami", "Tamika", "Tammi",
        "Tammie", "Tammy", "Tamra", "Tania", "Tanisha", "Tanya", "Tara", "Tasha", "Taylor", "Teresa", "Teri", "Terra", "Terri", "Terrie", "Terry", "Tessa",
        "Thelma", "Theresa", "Therese", "Tia", "Tiffany", "Tina", "Tisha", "Tommie", "Toni", "Tonia", "Tonya", "Tracey", "Traci", "Tracie", "Tracy", "Tricia",
        "Trina", "Trisha", "Trudy", "Twila", "Ursula", "Valarie", "Valeria", "Valerie", "Vanessa", "Velma", "Vera", "Verna", "Veronica", "Vicki", "Vickie", "Vicky",
        "Victoria", "Vilma", "Viola", "Violet", "Virgie", "Virginia", "Vivian", "Vonda", "Wanda", "Wendi", "Wendy", "Whitney", "Wilda", "Willa", "Willie", "Wilma",
        "Winifred", "Winnie", "Yesenia", "Yolanda", "Young", "Yvette", "Yvonne", "Zelma"
    };
    private static string[] MaleFirstNames = new string[] {
        "Aaron", "Abdul", "Abe", "Abel", "Abraham", "Abram", "Adam", "Adolfo",
        "Adrian", "Ahmed", "Al", "Alan", "Albert", "Alberto", "Alden", "Alec", "Alejandro", "Alex", "Alexander", "Alexis", "Alfonzo", "Alfred", "Allan", "Alonso",
        "Alonzo", "Alphonse", "Alphonso", "Alton", "Alva", "Alvaro", "Alvin", "Amado", "Ambrose", "Amos", "Anderson", "Andre", "Andrea", "Andres", "Andrew", "Andy",
        "Angel", "Angelo", "Anibal", "Anthony", "Antione", "Antoine", "Anton", "Antone", "Antonia", "Antonio", "Antony", "Antwan", "Archie", "Arden", "Ariel", "Arlen",
        "Arlie", "Armand", "Armando", "Arnold", "Arnoldo", "Arnulfo", "Aron", "Arron", "Art", "Arthur", "Arturo", "Asa", "Ashley", "Aubrey", "August", "Augustine",
        "Augustus", "Aurelio", "Austin", "Avery", "Barney", "Barrett", "Barry", "Bart", "Barton", "Basil", "Beau", "Ben", "Benedict", "Benito", "Benjamin", "Bennett",
        "Bennie", "Benny", "Benton", "Bernard", "Bernardo", "Bernie", "Berry", "Bert", "Bertram", "Bill", "Billie", "Billy", "Blaine", "Blair", "Blake", "Bo",
        "Bob", "Bob", "Bob", "Bob", "Bob", "Bob", "Bob", "Bob", "Bob", "Bobbie", "Bobby", "Booker", "Boris", "Boyce", "Boyd", "Brad",
        "Bradford", "Bradley", "Bradly", "Brady", "Brain", "Branden", "Brandon", "Brant", "Brendan", "Brendon", "Brent", "Brenton", "Bret", "Brett", "Brian", "Brice",
        "Britt", "Brock", "Broderick", "Brooks", "Bruce", "Bruno", "Bryan", "Bryant", "Bryce", "Bryon", "Buck", "Bud", "Buddy", "Buford", "Burl", "Burt",
        "Burton", "Buster", "Byron", "Caleb", "Calvin", "Cameron", "Carey", "Carl", "Carlo", "Carlos", "Carlton", "Carmelo", "Carmen", "Carmine", "Carol", "Carrol",
        "Carroll", "Carson", "Carter", "Cary", "Casey", "Cecil", "Cedric", "Cedrick", "Cesar", "Chad", "Chadwick", "Chance", "Chang", "Charles", "Charley", "Charlie",
        "Chas", "Chase", "Chauncey", "Chester", "Chet", "Chi", "Chong", "Chris", "Christian", "Christoper", "Christopher", "Chuck", "Chung", "Clair", "Clarence", "Clark",
        "Claud", "Claude", "Claudio", "Clay", "Clayton", "Clement", "Clemente", "Cleo", "Cletus", "Cleveland", "Cliff", "Clifford", "Clifton", "Clint", "Clinton", "Clyde",
        "Cody", "Colby", "Cole", "Coleman", "Colin", "Collin", "Colton", "Columbus", "Connie", "Conrad", "Cordell", "Corey", "Cornelius", "Cornell", "Cortez", "Cory",
        "Courtney", "Coy", "Craig", "Cristobal", "Cristopher", "Cruz", "Curt", "Curtis", "Cyril", "Cyrus", "Dale", "Dallas", "Dalton", "Damian", "Damien", "Damion",
        "Damon", "Dan", "Dana", "Dane", "Danial", "Daniel", "Danilo", "Dannie", "Danny", "Dante", "Darell", "Daren", "Darin", "Dario", "Darius", "Darnell",
        "Daron", "Darrel", "Darrell", "Darren", "Darrick", "Darrin", "Darron", "Darryl", "Darwin", "Daryl", "Dave", "David", "Davis", "Dean", "Deandre", "Deangelo",
        "Dee", "Del", "Delbert", "Delmar", "Delmer", "Demarcus", "Demetrius", "Denis", "Dennis", "Denny", "Denver", "Deon", "Derek", "Derick", "Derrick", "Deshawn",
        "Desmond", "Devin", "Devon", "Dewayne", "Dewey", "Dewitt", "Dexter", "Dick", "Diego", "Dillon", "Dino", "Dion", "Dirk", "Domenic", "Domingo", "Dominic",
        "Dominick", "Dominique", "Don", "Donald", "Dong", "Donn", "Donnell", "Donnie", "Donny", "Donovan", "Donte", "Dorian", "Dorsey", "Doug", "Douglas", "Douglass",
        "Doyle", "Drew", "Duane", "Dudley", "Duncan", "Dustin", "Dusty", "Dwain", "Dwayne", "Dwight", "Dylan", "Earl", "Earle", "Earnest", "Ed", "Eddie",
        "Eddy", "Edgar", "Edgardo", "Edison", "Edmond", "Edmund", "Edmundo", "Eduardo", "Edward", "Edwardo", "Edwin", "Efrain", "Efren", "Elbert", "Elden", "Eldon",
        "Eldridge", "Eli", "Elias", "Elijah", "Eliseo", "Elisha", "Elliot", "Elliott", "Ellis", "Ellsworth", "Elmer", "Elmo", "Eloy", "Elroy", "Elton", "Elvin",
        "Elvis", "Elwood", "Emanuel", "Emerson", "Emery", "Emil", "Emile", "Emilio", "Emmanuel", "Emmett", "Emmitt", "Emory", "Enoch", "Enrique", "Erasmo", "Eric",
        "Erich", "Erick", "Erik", "Erin", "Ernest", "Ernesto", "Ernie", "Errol", "Ervin", "Erwin", "Esteban", "Ethan", "Eugene", "Eugenio", "Eusebio", "Evan",
        "Everett", "Everette", "Ezekiel", "Ezequiel", "Ezra", "Fabian", "Faustino", "Fausto", "Federico", "Felipe", "Felix", "Felton", "Ferdinand", "Fermin", "Fernando", "Fidel",
        "Filiberto", "Fletcher", "Florencio", "Florentino", "Floyd", "Forest", "Forrest", "Foster", "Frances", "Francesco", "Francis", "Francisco", "Frank", "Frankie", "Franklin", "Franklyn",
        "Fred", "Freddie", "Freddy", "Frederic", "Frederick", "Fredric", "Fredrick", "Freeman", "Fritz", "Gabriel", "Gail", "Gale", "Galen", "Garfield", "Garland", "Garret",
        "Garrett", "Garry", "Garth", "Gary", "Gaston", "Gavin", "Gayle", "Gaylord", "Genaro", "Gene", "Geoffrey", "George", "Gerald", "Geraldo", "Gerard", "Gerardo",
        "German", "Gerry", "Gil", "Gilbert", "Gilberto", "Gino", "Giovanni", "Giuseppe", "Glen", "Glenn", "Gonzalo", "Gordon", "Grady", "Graham", "Graig", "Grant",
        "Granville", "Greg", "Gregg", "Gregorio", "Gregory", "Grover", "Guadalupe", "Guillermo", "Gus", "Gustavo", "Guy", "Hai", "Hal", "Hank", "Hans", "Harlan",
        "Harland", "Harley", "Harold", "Harris", "Harrison", "Harry", "Harvey", "Hassan", "Hayden", "Haywood", "Heath", "Hector", "Henry", "Herb", "Herbert", "Heriberto",
        "Herman", "Herschel", "Hershel", "Hilario", "Hilton", "Hipolito", "Hiram", "Hobert", "Hollis", "Homer", "Hong", "Horace", "Horacio", "Hosea", "Houston", "Howard",
        "Hoyt", "Hubert", "Huey", "Hugh", "Hugo", "Humberto", "Hung", "Hunter", "Hyman", "Ian", "Ignacio", "Ike", "Ira", "Irvin", "Irving", "Irwin",
        "Isaac", "Isaiah", "Isaias", "Isiah", "Isidro", "Ismael", "Israel", "Isreal", "Issac", "Ivan", "Ivory", "Jacinto", "Jack", "Jackie", "Jackson", "Jacob",
        "Jacques", "Jae", "Jaime", "Jake", "Jamaal", "Jamal", "Jamar", "Jame", "Jamel", "James", "Jamey", "Jamie", "Jamison", "Jan", "Jared", "Jarod",
        "Jarred", "Jarrett", "Jarrod", "Jarvis", "Jason", "Jasper", "Javier", "Jay", "Jayson", "Jc", "Jean", "Jed", "Jeff", "Jefferey", "Jefferson", "Jeffery",
        "Jeffrey", "Jeffry", "Jerald", "Jeramy", "Jere", "Jeremiah", "Jeremy", "Jermaine", "Jerold", "Jerome", "Jeromy", "Jerrell", "Jerrod", "Jerrold", "Jerry", "Jess",
        "Jesse", "Jessie", "Jesus", "Jewel", "Jewell", "Jim", "Jimmie", "Jimmy", "Joan", "Joaquin", "Jody", "Joe", "Joel", "Joesph", "Joey", "John",
        "Johnathan", "Johnathon", "Johnie", "Johnnie", "Johnny", "Johnson", "Jon", "Jonah", "Jonas", "Jonathan", "Jonathon", "Jordan", "Jordon", "Jorge", "Jose", "Josef",
        "Joseph", "Josh", "Joshua", "Josiah", "Jospeh", "Josue", "Juan", "Jude", "Judson", "Jules", "Julian", "Julio", "Julius", "Junior", "Justin", "Kareem",
        "Karl", "Kasey", "Keenan", "Keith", "Kelley", "Kelly", "Kelvin", "Ken", "Kendall", "Kendrick", "Keneth", "Kenneth", "Kennith", "Kenny", "Kent", "Kenton",
        "Kermit", "Kerry", "Keven", "Kevin", "Kieth", "Kim", "King", "Kip", "Kirby", "Kirk", "Korey", "Kory", "Kraig", "Kris", "Kristofer", "Kristopher",
        "Kurt", "Kurtis", "Kyle", "Lacy", "Lamar", "Lamont", "Lance", "Landon", "Lane", "Lanny", "Larry", "Lauren", "Laurence", "Lavern", "Laverne", "Lawerence",
        "Lawrence", "Lazaro", "Leandro", "Lee", "Leif", "Leigh", "Leland", "Lemuel", "Len", "Lenard", "Lenny", "Leo", "Leon", "Leonard", "Leonardo", "Leonel",
        "Leopoldo", "Leroy", "Les", "Lesley", "Leslie", "Lester", "Levi", "Lewis", "Lincoln", "Lindsay", "Lindsey", "Lino", "Linwood", "Lionel", "Lloyd", "Logan",
        "Lon", "Long", "Lonnie", "Lonny", "Loren", "Lorenzo", "Lou", "Louie", "Louis", "Lowell", "Loyd", "Lucas", "Luciano", "Lucien", "Lucio", "Lucius",
        "Luigi", "Luis", "Luke", "Lupe", "Luther", "Lyle", "Lyman", "Lyndon", "Lynn", "Lynwood", "Mac", "Mack", "Major", "Malcolm", "Malcom", "Malik",
        "Man", "Manual", "Manuel", "Marc", "Marcel", "Marcelino", "Marcellus", "Marcelo", "Marco", "Marcos", "Marcus", "Margarito", "Maria", "Mariano", "Mario", "Marion",
        "Mark", "Markus", "Marlin", "Marlon", "Marquis", "Marshall", "Martin", "Marty", "Marvin", "Mary", "Mason", "Mathew", "Matt", "Matthew", "Maurice", "Mauricio",
        "Mauro", "Max", "Maximo", "Maxwell", "Maynard", "Mckinley", "Mel", "Melvin", "Merle", "Merlin", "Merrill", "Mervin", "Micah", "Michael", "Michal", "Michale",
        "Micheal", "Michel", "Mickey", "Miguel", "Mike", "Mikel", "Milan", "Miles", "Milford", "Millard", "Milo", "Milton", "Minh", "Miquel", "Mitch", "Mitchel",
        "Mitchell", "Modesto", "Mohamed", "Mohammad", "Mohammed", "Moises", "Monroe", "Monte", "Monty", "Morgan", "Morris", "Morton", "Mose", "Moses", "Moshe", "Murray",
        "Myles", "Myron", "Napoleon", "Nathan", "Nathanael", "Nathanial", "Nathaniel", "Neal", "Ned", "Neil", "Nelson", "Nestor", "Neville", "Newton", "Nicholas", "Nick",
        "Nickolas", "Nicky", "Nicolas", "Nigel", "Noah", "Noble", "Noe", "Noel", "Nolan", "Norbert", "Norberto", "Norman", "Normand", "Norris", "Numbers", "Octavio",
        "Odell", "Odis", "Olen", "Olin", "Oliver", "Ollie", "Omar", "Omer", "Oren", "Orlando", "Orval", "Orville", "Oscar", "Osvaldo", "Oswaldo", "Otha",
        "Otis", "Otto", "Owen", "Pablo", "Palmer", "Paris", "Parker", "Pasquale", "Pat", "Patricia", "Patrick", "Paul", "Pedro", "Percy", "Perry", "Pete",
        "Peter", "Phil", "Philip", "Phillip", "Pierre", "Porfirio", "Porter", "Preston", "Prince", "Quentin", "Quincy", "Quinn", "Quintin", "Quinton", "Rafael", "Raleigh",
        "Ralph", "Ramiro", "Ramon", "Randal", "Randall", "Randell", "Randolph", "Randy", "Raphael", "Rashad", "Raul", "Ray", "Rayford", "Raymon", "Raymond", "Raymundo",
        "Reed", "Refugio", "Reggie", "Reginald", "Reid", "Reinaldo", "Renaldo", "Renato", "Rene", "Reuben", "Rex", "Rey", "Reyes", "Reynaldo", "Rhett", "Ricardo",
        "Rich", "Richard", "Richie", "Rick", "Rickey", "Rickie", "Ricky", "Rico", "Rigoberto", "Riley", "Rob", "Robbie", "Robby", "Robert", "Roberto", "Robin",
        "Robt", "Rocco", "Rocky", "Rod", "Roderick", "Rodger", "Rodney", "Rodolfo", "Rodrick", "Rodrigo", "Rogelio", "Roger", "Roland", "Rolando", "Rolf", "Rolland",
        "Roman", "Romeo", "Ron", "Ronald", "Ronnie", "Ronny", "Roosevelt", "Rory", "Rosario", "Roscoe", "Rosendo", "Ross", "Roy", "Royal", "Royce", "Ruben",
        "Rubin", "Rudolf", "Rudolph", "Rudy", "Rueben", "Rufus", "Rupert", "Russ", "Russel", "Russell", "Rusty", "Ryan", "Sal", "Salvador", "Salvatore", "Sam",
        "Sammie", "Sammy", "Samual", "Samuel", "Sandy", "Sanford", "Sang", "Santiago", "Santo", "Santos", "Saul", "Scot", "Scott", "Scottie", "Scotty", "Sean",
        "Sebastian", "Sergio", "Seth", "Seymour", "Shad", "Shane", "Shannon", "Shaun", "Shawn", "Shayne", "Shelby", "Sheldon", "Shelton", "Sherman", "Sherwood", "Shirley",
        "Shon", "Sid", "Sidney", "Silas", "Simon", "Sol", "Solomon", "Son", "Sonny", "Spencer", "Stacey", "Stacy", "Stan", "Stanford", "Stanley", "Stanton",
        "Stefan", "Stephan", "Stephen", "Sterling", "Steve", "Steven", "Stevie", "Stewart", "Stuart", "Sung", "Sydney", "Sylvester", "Tad", "Tanner", "Taylor", "Ted",
        "Teddy", "Teodoro", "Terence", "Terrance", "Terrell", "Terrence", "Terry", "Thad", "Thaddeus", "Thanh", "Theo", "Theodore", "Theron", "Thomas", "Thurman", "Tim",
        "Timmy", "Timothy", "Titus", "Tobias", "Toby", "Tod", "Todd", "Tom", "Tomas", "Tommie", "Tommy", "Toney", "Tony", "Tory", "Tracey", "Tracy",
        "Travis", "Trent", "Trenton", "Trevor", "Trey", "Trinidad", "Tristan", "Troy", "Truman", "Tuan", "Ty", "Tyler", "Tyree", "Tyrell", "Tyron", "Tyrone",
        "Tyson", "Ulysses", "Val", "Valentin", "Valentine", "Van", "Vance", "Vaughn", "Vern", "Vernon", "Vicente", "Victor", "Vince", "Vincent", "Vincenzo", "Virgil",
        "Virgilio", "Vito", "Von", "Wade", "Waldo", "Walker", "Wallace", "Wally", "Walter", "Walton", "Ward", "Warner", "Warren", "Waylon", "Wayne", "Weldon",
        "Wendell", "Werner", "Wes", "Wesley", "Weston", "Whitney", "Wilber", "Wilbert", "Wilbur", "Wilburn", "Wiley", "Wilford", "Wilfred", "Wilfredo", "Will", "Willard",
        "William", "Williams", "Willian", "Willie", "Willis", "Willy", "Wilmer", "Wilson", "Wilton", "Winford", "Winfred", "Winston", "Wm", "Woodrow", "Wyatt", "Xavier",
        "Yong", "Young", "Zachariah", "Zachary", "Zachery", "Zack", "Zackary", "Zane"
     };

    private static string[] LastNames = new string[] {
        "Abbott", "Acosta", "Adams", "Adkins", "Aguilar", "Albert", "Alexander", "Alford", "Allen", "Alston", "Alvarado", "Alvarez", "Anderson", "Andrews", "Anthony", "Armstrong",
        "Arnold", "Ashley", "Atkins", "Atkinson", "Austin", "Avery", "Ayala", "Ayers", "Bailey", "Baird", "Baker", "Baldwin", "Ball", "Ballard", "Banks", "Barber",
        "Barker", "Barlow", "Barnes", "Barnett", "Barr", "Barrera", "Barrett", "Barron", "Barry", "Bartlett", "Barton", "Bass", "Bates", "Battle", "Bauer", "Baxter",
        "Beach", "Bean", "Beard", "Beasley", "Beck", "Becker", "Bell", "Bender", "Benjamin", "Bennett", "Benson", "Bentley", "Benton", "Berg", "Berger", "Bernard",
        "Berry", "Best", "Bird", "Bishop", "Black", "Blackburn", "Blackwell", "Blair", "Blake", "Blanchard", "Blankenship", "Blevins", "Bolton", "Bond", "Bonner", "Booker",
        "Boone", "Booth", "Bowen", "Bowers", "Bowman", "Boyd", "Boyer", "Boyle", "Bradford", "Bradley", "Bradshaw", "Brady", "Branch", "Bray", "Brennan", "Brewer",
        "Bridges", "Briggs", "Bright", "Britt", "Brock", "Brooks", "Brown", "Browning", "Bruce", "Bryan", "Bryant", "Buchanan", "Buck", "Buckley", "Buckner", "Bullock",
        "Burch", "Burgess", "Burke", "Burks", "Burnett", "Burns", "Burris", "Burt", "Burton", "Bush", "Butler", "Byers", "Byrd", "Cabrera", "Cain", "Calderon",
        "Caldwell", "Calhoun", "Callahan", "Camacho", "Cameron", "Campbell", "Campos", "Cannon", "Cantrell", "Cantu", "Cardenas", "Carey", "Carlson", "Carney", "Carpenter", "Carr",
        "Carrillo", "Carroll", "Carson", "Carter", "Carver", "Case", "Casey", "Cash", "Castaneda", "Castillo", "Castro", "Cervantes", "Chambers", "Chan", "Chandler", "Chaney",
        "Chang", "Chapman", "Charles", "Chase", "Chavez", "Chen", "Cherry", "Christensen", "Christian", "Church", "Clark", "Clarke", "Clay", "Clayton", "Clements", "Clemons",
        "Cleveland", "Cline", "Cobb", "Cochran", "Coffey", "Cohen", "Cole", "Coleman", "Collier", "Collins", "Colon", "Combs", "Compton", "Conley", "Conner", "Conrad",
        "Contreras", "Conway", "Cook", "Cooke", "Cooley", "Cooper", "Copeland", "Cortez", "Cote", "Cotton", "Cox", "Craft", "Craig", "Crane", "Crawford", "Crosby",
        "Cross", "Cruz", "Cummings", "Cunningham", "Curry", "Curtis", "Dale", "Dalton", "Daniel", "Daniels", "Daugherty", "Davenport", "David", "Davidson", "Davis", "Dawson",
        "Day", "Dean", "Decker", "Dejesus", "Delacruz", "Delaney", "Deleon", "Delgado", "Dennis", "Diaz", "Dickerson", "Dickson", "Dillard", "Dillon", "Dixon", "Dodson",
        "Dominguez", "Donaldson", "Donovan", "Dorsey", "Dotson", "Douglas", "Downs", "Doyle", "Drake", "Dudley", "Duffy", "Duke", "Duncan", "Dunlap", "Dunn", "Duran",
        "Durham", "Dyer", "Eaton", "Edwards", "Elliott", "Ellis", "Ellison", "Emerson", "England", "English", "Erickson", "Espinoza", "Estes", "Estrada", "Evans", "Everett",
        "Ewing", "Farley", "Farmer", "Farrell", "Faulkner", "Ferguson", "Fernandez", "Ferrell", "Fields", "Figueroa", "Finch", "Finley", "Fischer", "Fisher", "Fitzgerald", "Fitzpatrick",
        "Fleming", "Fletcher", "Flores", "Flowers", "Floyd", "Flynn", "Foley", "Forbes", "Ford", "Foreman", "Foster", "Fowler", "Fox", "Francis", "Franco", "Frank",
        "Franklin", "Franks", "Frazier", "Frederick", "Freeman", "French", "Frost", "Fry", "Frye", "Fuentes", "Fuller", "Fulton", "Gaines", "Gallagher", "Gallegos", "Galloway",
        "Gamble", "Garcia", "Gardner", "Garner", "Garrett", "Garrison", "Garza", "Gates", "Gay", "Gentry", "George", "Gibbs", "Gibson", "Gilbert", "Giles", "Gill",
        "Gillespie", "Gilliam", "Gilmore", "Glass", "Glenn", "Glover", "Goff", "Golden", "Gomez", "Gonzales", "Gonzalez", "Good", "Goodman", "Goodwin", "Gordon", "Gould",
        "Graham", "Grant", "Graves", "Gray", "Green", "Greene", "Greer", "Gregory", "Griffin", "Griffith", "Grimes", "Gross", "Guerra", "Guerrero", "Guthrie", "Gutierrez",
        "Guy", "Guzman", "Hahn", "Hale", "Haley", "Hall", "Hamilton", "Hammond", "Hampton", "Hancock", "Haney", "Hansen", "Hanson", "Hardin", "Harding", "Hardy",
        "Harmon", "Harper", "Harrell", "Harrington", "Harris", "Harrison", "Hart", "Hartman", "Harvey", "Hatfield", "Hawkins", "Hayden", "Hayes", "Haynes", "Hays", "Head",
        "Heath", "Hebert", "Henderson", "Hendricks", "Hendrix", "Henry", "Hensley", "Henson", "Herman", "Hernandez", "Herrera", "Herring", "Hess", "Hester", "Hewitt", "Hickman",
        "Hicks", "Higgins", "Hill", "Hines", "Hinton", "Hobbs", "Hodge", "Hodges", "Hoffman", "Hogan", "Holcomb", "Holden", "Holder", "Holland", "Holloway", "Holman",
        "Holmes", "Holt", "Hood", "Hooper", "Hoover", "Hopkins", "Hopper", "Horn", "Horne", "Horton", "House", "Houston", "Howard", "Howe", "Howell", "Hubbard",
        "Huber", "Hudson", "Huff", "Huffman", "Hughes", "Hull", "Humphrey", "Hunt", "Hunter", "Hurley", "Hurst", "Hutchinson", "Hyde", "Ingram", "Irwin", "Jackson",
        "Jacobs", "Jacobson", "James", "Jarvis", "Jefferson", "Jenkins", "Jennings", "Jensen", "Jimenez", "Johns", "Johnson", "Johnston", "Jones", "Jordan", "Joseph", "Joyce",
        "Joyner", "Juarez", "Justice", "Kane", "Kaufman", "Keith", "Keller", "Kelley", "Kelly", "Kemp", "Kennedy", "Kent", "Kerr", "Key", "Kidd", "Kim",
        "King", "Kinney", "Kirby", "Kirk", "Kirkland", "Klein", "Kline", "Knapp", "Knight", "Knowles", "Knox", "Koch", "Kramer", "Lamb", "Lambert", "Lancaster",
        "Landry", "Lane", "Lang", "Langley", "Lara", "Larsen", "Larson", "Lawrence", "Lawson", "Le", "Leach", "Leblanc", "Lee", "Leon", "Leonard", "Lester",
        "Levine", "Levy", "Lewis", "Lindsay", "Lindsey", "Little", "Livingston", "Lloyd", "Logan", "Long", "Lopez", "Lott", "Love", "Lowe", "Lowery", "Lucas",
        "Luna", "Lynch", "Lynn", "Lyons", "Macdonald", "Macias", "Mack", "Madden", "Maddox", "Maldonado", "Malone", "Mann", "Manning", "Marks", "Marquez", "Marsh",
        "Marshall", "Martin", "Martinez", "Mason", "Massey", "Mathews", "Mathis", "Matthews", "Maxwell", "May", "Mayer", "Maynard", "Mayo", "Mays", "McBride", "McCall",
        "McCarthy", "McCarty", "McClain", "McClure", "McConnell", "McCormick", "McCoy", "McCray", "McCullough", "McDaniel", "McDonald", "McDowell", "McFadden", "McFarland", "McGee", "McGowan",
        "McGuire", "McIntosh", "McIntyre", "McKay", "McKee", "McKenzie", "McKinney", "McKnight", "McLaughlin", "Mclean", "McLeod", "McMahon", "McMillan", "McNeil", "McPherson", "Meadows",
        "Medina", "Mejia", "Melendez", "Melton", "Mendez", "Mendoza", "Mercado", "Mercer", "Merrill", "Merritt", "Meyer", "Meyers", "Michael", "Middleton", "Miles", "Miller",
        "Mills", "Miranda", "Mitchell", "Molina", "Monroe", "Montgomery", "Montoya", "Moody", "Moon", "Mooney", "Moore", "Morales", "Moran", "Moreno", "Morgan", "Morin",
        "Morris", "Morrison", "Morrow", "Morse", "Morton", "Moses", "Mosley", "Moss", "Mueller", "Mullen", "Mullins", "Munoz", "Murphy", "Murray", "Myers", "Nash",
        "Navarro", "Neal", "Nelson", "Newman", "Newton", "Nguyen", "Nichols", "Nicholson", "Nielsen", "Nieves", "Nixon", "Noble", "Noel", "Nolan", "Norman", "Norris",
        "Norton", "Nunez", "OBrien", "Ochoa", "OConnor", "Odom", "Odonnell", "Oliver", "Olsen", "Olson", "Oneal", "Oneil", "Oneill", "Orr", "Ortega", "Ortiz",
        "Osborn", "Osborne", "Owen", "Owens", "Pace", "Pacheco", "Padilla", "Page", "Palmer", "Park", "Parker", "Parks", "Parrish", "Parsons", "Pate", "Patel",
        "Patrick", "Patterson", "Patton", "Paul", "Payne", "Pearson", "Peck", "Pena", "Pennington", "Perez", "Perkins", "Perry", "Peters", "Petersen", "Peterson", "Petty",
        "Phelps", "Phillips", "Pickett", "Pierce", "Pittman", "Pitts", "Pollard", "Poole", "Pope", "Porter", "Potter", "Potts", "Powell", "Powers", "Pratt", "Preston",
        "Price", "Prince", "Pruitt", "Puckett", "Pugh", "Quinn", "Ramirez", "Ramos", "Ramsey", "Randall", "Randolph", "Rasmussen", "Ratliff", "Ray", "Raymond", "Reed",
        "Reese", "Reeves", "Reid", "Reilly", "Reyes", "Reynolds", "Rhodes", "Rice", "Rich", "Richard", "Richards", "Richardson", "Richmond", "Riddle", "Riggs", "Riley",
        "Rios", "Rivas", "Rivera", "Rivers", "Roach", "Robbins", "Roberson", "Roberts", "Robertson", "Robinson", "Robles", "Rocha", "Rodgers", "Rodriguez", "Rodriquez", "Rogers",
        "Rojas", "Rollins", "Roman", "Romero", "Rosa", "Rosales", "Rosario", "Rose", "Ross", "Roth", "Rowe", "Rowland", "Roy", "Ruiz", "Rush", "Russell",
        "Russo", "Rutledge", "Ryan", "Salas", "Salazar", "Salinas", "Sampson", "Sanchez", "Sanders", "Sandoval", "Sanford", "Santana", "Santiago", "Santos", "Sargent", "Saunders",
        "Savage", "Sawyer", "Schmidt", "Schneider", "Schroeder", "Schultz", "Schwartz", "Scott", "Sears", "Sellers", "Serrano", "Sexton", "Shaffer", "Shannon", "Sharp", "Sharpe",
        "Shaw", "Shelton", "Shepard", "Shepherd", "Sheppard", "Sherman", "Shields", "Short", "Silva", "Simmons", "Simon", "Simpson", "Sims", "Singleton", "Skinner", "Slater",
        "Sloan", "Small", "Smith", "Snider", "Snow", "Snyder", "Solis", "Solomon", "Sosa", "Soto", "Sparks", "Spears", "Spence", "Spencer", "Stafford", "Stanley",
        "Stanton", "Stark", "Steele", "Stein", "Stephens", "Stephenson", "Stevens", "Stevenson", "Stewart", "Stokes", "Stone", "Stout", "Strickland", "Strong", "Stuart", "Suarez",
        "Sullivan", "Summers", "Sutton", "Swanson", "Sweeney", "Sweet", "Sykes", "Talley", "Tanner", "Tate", "Taylor", "Terrell", "Terry", "Thomas", "Thompson", "Thornton",
        "Tillman", "Todd", "Torres", "Townsend", "Tran", "Travis", "Trevino", "Trujillo", "Tucker", "Turner", "Tyler", "Tyson", "Underwood", "Valdez", "Valencia", "Valentine",
        "Valenzuela", "Vance", "Vang", "Vargas", "Vasquez", "Vaughan", "Vaughn", "Vazquez", "Vega", "Velasquez", "Velazquez", "Velez", "Villarreal", "Vincent", "Vinson", "Wade",
        "Wagner", "Walker", "Wall", "Wallace", "Waller", "Walls", "Walsh", "Walter", "Walters", "Walton", "Ward", "Ware", "Warner", "Warren", "Washington", "Waters",
        "Watkins", "Watson", "Watts", "Weaver", "Webb", "Weber", "Webster", "Weeks", "Weiss", "Welch", "Wells", "West", "Wheeler", "Whitaker", "White", "Whitehead",
        "Whitfield", "Whitley", "Whitney", "Wiggins", "Wilcox", "Wilder", "Wiley", "Wilkerson", "Wilkins", "Wilkinson", "William", "Williams", "Williamson", "Willis", "Wilson", "Winters",
        "Wise", "Witt", "Wolf", "Wolfe", "Wong", "Wood", "Woodard", "Woods", "Woodward", "Wooten", "Workman", "Wright", "Wyatt", "Wynn", "Yang", "Yates",
        "York", "Young", "Zamora", "Zimmerman"
     };

    #endregion

    public static IEnumerable<ContributionData> GetContributionList() {

      contributionListsCount += 1;

      if (contributionListsCount == 7) { SetContributionGrowthPhase(2); }
      if (contributionListsCount == 21) { SetContributionGrowthPhase(3); }
      if (contributionListsCount == 30) { SetContributionGrowthPhase(4); }
      if (contributionListsCount == 45) { SetContributionGrowthPhase(5); }
      if (contributionListsCount == 60) { SetContributionGrowthPhase(6); }
      if (contributionListsCount == 85) { SetContributionGrowthPhase(7); }
      if (contributionListsCount == 100) { SetContributionGrowthPhase(8); }
      if (contributionListsCount == 120) { SetContributionGrowthPhase(9); }
      if (contributionListsCount == 135) { SetContributionGrowthPhase(10); }

      int ContributionCount = 1;

      switch (ContributionGrowthPhase) {
        case 1:
          ContributionCount = 1;
          break;
        case 2:
          ContributionCount = 8;
          break;
        case 3:
          ContributionCount = RandomNumberFactory.Next(15, 30);
          break;
        case 4:
          ContributionCount = RandomNumberFactory.Next(22, 30);
          break;
        case 5:
          ContributionCount = RandomNumberFactory.Next(28, 42);
          break;
        case 6:
          ContributionCount = RandomNumberFactory.Next(1, 2);
          break;
        case 7:
          ContributionCount = RandomNumberFactory.Next(34, 48);
          break;
        case 8:
          ContributionCount = RandomNumberFactory.Next(42, 62);
          break;
        case 9:
          ContributionCount = RandomNumberFactory.Next(44, 70);
          break;
        case 10:
          ContributionCount = RandomNumberFactory.Next(56, 66);
          break;
      }

      List<ContributionData> list = new List<ContributionData>(ContributionCount);
      for (int i = 1; i <= ContributionCount; i++) {
        list.Add(GetNextContribution());
      }

      Console.Write(".");
      return list;
    }

    private static LocationData GetNextContributionLocation() {

      LocationData[] contributionLocations;

      switch (ContributionGrowthPhase) {
        case 1:
          contributionLocations = ContributionLocations1;
          break;
        case 2:
          contributionLocations = ContributionLocations2;
          break;
        case 3:
          contributionLocations = ContributionLocations3;
          break;
        case 4:
          contributionLocations = ContributionLocations4;
          break;
        case 5:
          contributionLocations = ContributionLocations5;
          break;
        case 6:
          contributionLocations = ContributionLocations6;
          break;
        case 7:
          contributionLocations = ContributionLocations7;
          break;
        case 8:
          contributionLocations = ContributionLocations8;
          break;
        case 9:
          contributionLocations = ContributionLocations9;
          break;
        case 10:
          contributionLocations = ContributionLocations10;
          break;
        default:
          contributionLocations = ContributionLocations10;
          break;
      }
      int index = RandomNumberFactory.Next(0, contributionLocations.Length);
      return contributionLocations[index];
    }

    private static string GetNextFemaleFirstName() {
      int index = RandomNumberFactory.Next(0, FemaleFirstNames.Length);
      return FemaleFirstNames[index];
    }

    private static string GetNextMaleFirstName() {
      int index = RandomNumberFactory.Next(0, MaleFirstNames.Length);
      return MaleFirstNames[index];
    }

    private static string GetNextLastName() {
      int index = RandomNumberFactory.Next(0, LastNames.Length);
      return LastNames[index];
    }

    private static decimal GetNextContributionAmount(int Zipcode) {

      if (ContributionGrowthPhase == 6) {
        return 25;
      }

      if (Zipcode.Equals(33621)) {
        return 50;
      }

      decimal[] contributionAmounts = ContributionAmounts;

      int[] BiggerContributors = { 33609, 33626, 33612, 33760, 33613, 33759, 33765, 33619, 33647 };
      if (BiggerContributors.Contains(Zipcode)) {
        contributionAmounts = BiggerContributionAmounts;
      }

      int[] BigShotContributors = { 33629, 33621, 33713, 33709, 33760, 36299, 33606, 33709, 33713, 33710, 33770 };
      if (BigShotContributors.Contains(Zipcode)) {
        contributionAmounts = BigShotContributionAmounts;
      }

      int index = RandomNumberFactory.Next(0, contributionAmounts.Length);
      return contributionAmounts[index];
    }

    private static decimal[] ContributionAmounts = new decimal[] {
      25, 25, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 75, 75, 75, 75, 75, 75, 100, 100, 125, 150, 150, 150, 150, 150, 200, 200, 200, 500, 1000
    };

    private static decimal[] BiggerContributionAmounts = new decimal[] {
      25, 50, 50, 100, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 150, 250, 250, 250, 250, 250, 250, 250, 250, 250, 500, 500, 1000, 1000, 2500
    };

    private static decimal[] BigShotContributionAmounts = new decimal[] {
      100, 250, 250, 250, 250, 250, 250, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 1000, 1000, 1000, 1000, 2500, 2500, 5000, 10000
    };

  }


}
