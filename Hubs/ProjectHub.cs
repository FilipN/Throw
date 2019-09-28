using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Throw.Hubs
{
    public class ProjectHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {

            string[] sp = groupName.Split('?');
            string gid = sp[0];
            Guid g;
            bool validGuid = Guid.TryParse(gid, out g);
            if(validGuid)
                await Groups.AddToGroupAsync(Context.ConnectionId, gid);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task ChangePosition(string user, string projectid, string position)
        {
            await Clients.All.SendAsync("UpdatePosition", user, position);
        }

        public async Task ChangeLine(string user, string projectid,string snapshotid, string lineNumber,string lineText,bool newLine)
        {
            //provera prava
            //provera da li projectid i snapshotid odgovaraju nekom aktivnom projektu
            //provera da li je linija zakljucana od strane nekog drugog (zakljucana je ako neko na njoj radi), ukoliko jeste odbija se promena uz poruku o konfliktu

            //promena aktivnog snapshot-a 
            //ako je novi red u pitanju, menjamo pozicije i zakljucavanja svih korisnika, dodajemo red u niz linija koje oznacavaju snapshot

            //javljanje svima na projektu da je doslo do izmene linije (salje svima celu liniju)
            //refresh zakljucanosti linije kod svih korisnika (da ne mogu da edituju)

            //cuvanje snapshot-a u bazi ukoliko je poslednja verzija starija od 5 minuta (preko taska, nikako cekajuca naredba)
            await Clients.All.SendAsync("StateUpdate", user, "newline:bool","lineNumber","lineText","[positions]");
        }

        public async Task RevertToSnapshot(string user, string projectid, string position)
        {
            //brisanje svih pozicija
            //ucitavanje snapshot-a i postavljanje aktivnog (tako se dobija novi snapshotID)

            //slanje poruke svima da ucitaju ponovo projekat (naravno automatski)
            await Clients.All.SendAsync("RefreshProject", user, position);
        }

        public async Task ExecuteSnapshot(string user, string projectid)
        {
            //provera prava
            //pokretanje aktivnog snapshot-a
            //javljanje rezultata svim prikacenim korisnicima
            string executionResult = "";

            await Clients.All.SendAsync("ExecutionStateUpdate", user, executionResult);
        }

        public async Task ChatSendMessage(string user, string projectid, string message)
        {
            //brisanje svih pozicija
            //ucitavanje snapshot-a i postavljanje aktivnog (tako se dobija novi snapshotID)

            //slanje poruke svima da ucitaju ponovo projekat (naravno automatski)
            await Clients.All.SendAsync("RecieveMessage", user, message);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //uklanjanje korisnika Context.ConnectionId koji se diskonektovao
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

    }
}