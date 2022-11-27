//The following sample uses the Cryptography class to coduct the ticket draw for lunc burn aus lotto

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public class Delegation
{
    public string address;
    public double amount;
}

public class DelegationDetails
{
    public List<Delegation> delegators ;
}

public class TicketEntry
{
    public string Address { get; set; }
    public int EntryNumber { get; set; }
}

public class LuncBurnAusLottoDraw
{
    static HttpClient client = new HttpClient();
    static string url = "https://fcd.terra.dev/v1/staking/validators/terravaloper1kewjknvtym2lesr4qdldqc36cfrt8s432zmvhh/delegators?page=1&limit=5000";

    // Main method.
    public static void Main()
    {

        Console.WriteLine("########## LUNC BURN AUS LOTTO ############");
        Console.WriteLine(string.Format("Draw date/time - {0}\n", DateTime.Now));
        Console.WriteLine("LOTTO Rules");
        Console.WriteLine("-----------");
        Console.WriteLine("Every delegation of 100K LUNC to LUNC BURN AUS LOTTO Delegator earns an entry ticket to the draw\n");
        Console.WriteLine("Draw Process");
        Console.WriteLine("------------");
        Console.WriteLine("**** Last Ticket Wins ******");
        Console.WriteLine("Each ticket will be randomly removed using C# Cryptographic Random generator until the final ticket. The last standing ticket will be declared the winner\n");



        //Initiate ticket entries
        List<TicketEntry> ticketEntries = new List<TicketEntry>();

        //Get the delegation details from the LUNC blockchain
        var delegationDetails = GetDelegationDetailsAsync().Result;
        int entryNumber = 0;

        //Set the ticket entries based on delegation details
        foreach(Delegation delegation in delegationDetails.delegators)
        {
            var numberOfEntriesForAddress = Math.Floor(delegation.amount / 100000000000);
            for (int entryNumberOfAddress = 1; entryNumberOfAddress <= numberOfEntriesForAddress; entryNumberOfAddress++)
            {
                ticketEntries.Add(new TicketEntry { Address = delegation.address, EntryNumber = ++entryNumber });
            }
        }

        Console.WriteLine("Shuffling entries to increase Randomization");
        Console.WriteLine("------------");

        ticketEntries = Shuffle(ticketEntries);

        Console.WriteLine("Begin Draw");
        Console.WriteLine("------------");
        Console.WriteLine(string.Format("Number of total entries - {0} \n",ticketEntries.Count));

        
        

        Console.WriteLine("****** Starting removal of tickets randomly until last one left ***********");

        while (ticketEntries.Count()>1)
        {
            var min = 0;
            var max = ticketEntries.Count();
            var firstItem = ticketEntries.ElementAt(0);
            var lastItem = ticketEntries.ElementAt(ticketEntries.Count() - 1);

            var randomNumber = RandomNumberGenerator.GetInt32(min, max);
            var ticketToBeRemoved = ticketEntries[randomNumber];
            Console.WriteLine(string.Format("Removing Ticket Number {0} of Address {1}", ticketToBeRemoved.EntryNumber, ticketToBeRemoved.Address));
            ticketEntries.RemoveAt(randomNumber);
            Console.WriteLine(string.Format("Number of remaining entries - {0} \n", ticketEntries.Count));
        }

        //Last ticket standing
        var winner = ticketEntries[0];

        Console.WriteLine("*********** Only one ticket remaining *************");
        Console.WriteLine("*********** The winner is *************");
        Console.WriteLine("************************");
        Console.WriteLine("***********************");
        Console.WriteLine("**********************");
        Console.WriteLine("*********************");
        Console.WriteLine("*******************");
        Console.WriteLine("*****************");
        Console.WriteLine("****************");
        Console.WriteLine("***************");
        Console.WriteLine("**************");
        Console.WriteLine("*************");
        Console.WriteLine("************");
        Console.WriteLine("***********");
        Console.WriteLine("**********");
        Console.WriteLine("*********");
        Console.WriteLine("********");
        Console.WriteLine("*******");
        Console.WriteLine("******");
        Console.WriteLine("*****");
        Console.WriteLine("****");
        Console.WriteLine("***");
        Console.WriteLine("**");
        Console.WriteLine("*");

        Console.WriteLine(string.Format("Congratulations address {0} with ticket number {1}", winner.Address, winner.EntryNumber));


    }

    static async Task<DelegationDetails> GetDelegationDetailsAsync()
    {
        // Update port # in the following line.
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        DelegationDetails delegationDetails = null;
        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var delegationsAsString = await response.Content.ReadAsStringAsync();
            delegationDetails = JsonConvert.DeserializeObject<DelegationDetails>(delegationsAsString);
        }
        return delegationDetails;
    }

    static List<TicketEntry> Shuffle(List<TicketEntry> ticketEntries)
    {
        Random rand = new Random();
        var shuffledTicketEntries = ticketEntries.OrderBy(_ => rand.Next()).ToList();

        return shuffledTicketEntries;
    }
}