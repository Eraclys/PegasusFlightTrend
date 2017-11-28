﻿using FlightTrend.Core.FlightFinders;
using FlightTrend.Core.Specifications;
using JetBrains.Annotations;
using NodaTime;
using System.Collections.Generic;
using System.Linq;

namespace FlightTrend.Queries
{
    public static class Criteria
    {
        [NotNull]
        public static FindCheapestReturnFlightsForMultipleTravelDatesCriteria DefaultCriteria()
        {
            var departureFlightSpecification = new DepartureTimeIsAfter(new LocalTime(21, 00));
            var returnFlightSpecification = new DepartureTimeIsAfter(new LocalTime(21, 00));

            return FindCheapestReturnFlightsForMultipleTravelDatesCriteriaBuilder.New()
                .From("STN")
                .To("SAW")
                .FilterDepartureWith(departureFlightSpecification)
                .FilterReturnWith(returnFlightSpecification)
                .TravellingDates(EveryWeekendForTheNextFiveMonths().NotInside(BookedFlights()).ToArray())
                .Build();
        }

        [NotNull]
        public static IEnumerable<ReturnFlightDates> NotInside(this IEnumerable<ReturnFlightDates> values,
            IEnumerable<ReturnFlightDates> rangesToRemove)
        {
            return values
                .Where(x => !rangesToRemove.Any(r => r.DepartureDate <= x.DepartureDate && x.ReturnDate <= r.ReturnDate));
        }


        [ItemNotNull]
        private static IEnumerable<ReturnFlightDates> BookedFlights()
        {
            yield return new ReturnFlightDates(new LocalDate(2017,12,01), new LocalDate(2017,12,03));
            yield return new ReturnFlightDates(new LocalDate(2017, 12, 22), new LocalDate(2018, 01, 01));
            yield return new ReturnFlightDates(new LocalDate(2018, 01, 26), new LocalDate(2018, 01, 28));
            yield return new ReturnFlightDates(new LocalDate(2018, 02, 23), new LocalDate(2018, 02, 25));
        }

        [ItemNotNull]
        private static IEnumerable<ReturnFlightDates> EveryWeekendForTheNextFiveMonths()
        {
            var startDate = SystemClock.Instance.GetCurrentInstant().InUtc().Date;

            for (var i = 0; i < 22; i++)
            {
                var friday = startDate.Next(IsoDayOfWeek.Friday);
                var sunday = friday.Next(IsoDayOfWeek.Sunday);

                yield return new ReturnFlightDates(friday, sunday);

                startDate = sunday;
            }
        }
    }
}