using AutoMapper;
using System;
using System.Threading.Tasks;
using VacationsManager.Data.Entities;
using VacationsManager.Shared.Dtos;
using VacationsManager.Shared.Repos.Contracts;
using Microsoft.EntityFrameworkCore;
using VacationsManager.Shared.Attributes;
using YourNamespace.Data.Repos;

namespace VacationsManager.Data.Repos
{
    [AutoBind]
    public class TeamRepository : BaseRepository<Team, TeamDto>, ITeamRepository
    {
        public TeamRepository(VacationsManagerDbContext context, IMapper mapper) : base(context, mapper) { }

        // Метод за създаване на Team и актуализиране на потребителя (Team Leader)
        //public async Task CreateAndAssignTeamLeaderAsync(TeamDto teamDto)
        //{
        //    // Създаваме новия екип
        //    var team = _mapper.Map<Team>(teamDto);

        //    // Записваме екипа в базата данни
        //    await _context.Set<Team>().AddAsync(team);
        //    await _context.SaveChangesAsync();

        //    // Вземаме ID-то на новосъздадения екип
        //    var newTeamId = team.Id;

        //    // Вземаме TeamLeaderId на новия екип
        //    var teamLeaderId = team.TeamLeaderId;

        //    // Логване за отстраняване на грешки
        //    Console.WriteLine($"Team created with ID: {newTeamId}, assigning leader with ID: {teamLeaderId}");

        //    // Търсим съответния Team Leader в таблицата Users по TeamLeaderId
        //    var teamLeader = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == teamLeaderId);

        //    if (teamLeader == null)
        //    {
        //        Console.WriteLine($"User with ID {teamLeaderId} not found.");
        //        throw new ArgumentException("Team leader not found.");
        //    }

        //    // Актуализираме TeamId на TeamLeader-а
        //    teamLeader.TeamId = newTeamId;

        //    // Обновяваме потребителя (TeamLeader)
        //    _context.Set<User>().Update(teamLeader);

        //    // Записваме промените за TeamLeader
        //    await _context.SaveChangesAsync();

        //    // Логване за успешното актуализиране на TeamLeader
        //    Console.WriteLine($"Team leader with ID {teamLeaderId} now assigned to team with ID {newTeamId}");
        //}
    }
}
