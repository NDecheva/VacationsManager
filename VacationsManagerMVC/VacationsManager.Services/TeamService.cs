using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using VacationsManager.Shared.Attributes;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using VacationsManager.Shared.Services.Contracts;

namespace VacationsManager.Services
{
    [AutoBind]
    public class TeamService : BaseCrudService<TeamDto, ITeamRepository>, ITeamService
    {
        private readonly IUserService _userService;

        public TeamService(ITeamRepository repository, IUserService userService) : base(repository)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task AddDeveloperToTeamAsync(int teamId, int userId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null) throw new ArgumentException("Team not found.");

            var user = await _userService.GetByIdIfExistsAsync(userId); 
            if (user == null) throw new ArgumentException("User not found.");

            if (team.Developers == null)
                team.Developers = new List<UserDto>();

            if (team.Developers.Any(d => d.Id == userId))
                throw new InvalidOperationException("User is already a developer in this team.");

            team.Developers.Add(user);
            await _repository.SaveAsync(team);
        }

        public async Task RemoveDeveloperFromTeamAsync(int teamId, int userId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
                throw new ArgumentException("Team not found.");

            var developer = team.Developers?.FirstOrDefault(d => d.Id == userId);
            if (developer == null)
                throw new ArgumentException("Developer not found in this team.");

            developer.TeamId = null; 

            await _repository.SaveAsync(team);
            var updatedTeam = await _repository.GetByIdAsync(teamId);
            Console.WriteLine($"Updated TeamId is: {updatedTeam.Id}");

        }


    }

}