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
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository repository, IUserService userService, IUserRepository userRepository) : base(repository)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userRepository = userRepository;
        }

        public async Task AddDeveloperToTeamAsync(int teamId, int userId)
        {
            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
                throw new ArgumentException("Team not found.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (user.TeamId == teamId)
                throw new InvalidOperationException("User is already a member of this team.");

            user.TeamId = teamId;

            await _userRepository.SaveAsync(user);
        }


        public async Task RemoveDeveloperFromTeamAsync(int teamId, int userId)
        {

            var team = await _repository.GetByIdAsync(teamId);
            if (team == null)
                throw new ArgumentException("Team not found.");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found.");

            if (user.TeamId != teamId)
                throw new InvalidOperationException("User is not a member of this team.");

            user.TeamId = null;

            await _userRepository.SaveAsync(user); 
        }

        //public async Task CreateAndAssignTeamLeaderAsync(TeamDto teamDto)
        //{
        //    await _repository.CreateAndAssignTeamLeaderAsync(teamDto);
        //}

    }

}