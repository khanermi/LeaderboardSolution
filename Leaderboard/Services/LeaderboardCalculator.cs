using Leaderboard.Models;

namespace Leaderboard.Services;

public class LeaderboardCalculator : ILeaderboardCalculator
{
    private const int PodiumPlacesCount = 3;
    private const int FirstPlaceAfterPodium = PodiumPlacesCount + 1;

    public IReadOnlyList<UserWithPlace> CalculatePlaces(
        IReadOnlyList<IUserWithScore> usersWithScores,
        LeaderboardMinScores leaderboardMinScores)
    {
        if (usersWithScores.Count == 0)
        {
            return Array.Empty<UserWithPlace>();
        }

        var result = new List<UserWithPlace>(usersWithScores.Count);
        var sortedUsers = SortUsersByScoreDescending(usersWithScores);
        var podiumUserIds = new HashSet<long>(PodiumPlacesCount);

        AssignFirstPlace(sortedUsers, leaderboardMinScores, result, podiumUserIds);
        AssignSecondPlace(sortedUsers, leaderboardMinScores, result, podiumUserIds);
        AssignThirdPlace(sortedUsers, leaderboardMinScores, result, podiumUserIds);
        AssignRemainingPlaces(sortedUsers, result, podiumUserIds);

        return result;
    }

    private static List<IUserWithScore> SortUsersByScoreDescending(IReadOnlyList<IUserWithScore> users)
    {
        return users.OrderByDescending(u => u.Score).ToList();
    }

    private static void AssignFirstPlace(
        List<IUserWithScore> sortedUsers,
        LeaderboardMinScores minScores,
        List<UserWithPlace> result,
        HashSet<long> podiumUserIds)
    {
        var firstPlaceUser = sortedUsers
            .FirstOrDefault(u => u.Score >= minScores.FirstPlaceMinScore);
        if (firstPlaceUser == null)
        {
            return;
        }
        
        result.Add(new UserWithPlace(firstPlaceUser.UserId, 1));
        podiumUserIds.Add(firstPlaceUser.UserId);
    }

    private static void AssignSecondPlace(
        List<IUserWithScore> sortedUsers,
        LeaderboardMinScores minScores,
        List<UserWithPlace> result,
        HashSet<long> podiumUserIds)
    {
        var secondPlaceUser = sortedUsers.FirstOrDefault(u =>
            u.Score >= minScores.SecondPlaceMinScore &&
            !podiumUserIds.Contains(u.UserId));
        if (secondPlaceUser == null)
        {
            return;
        }
        
        result.Add(new UserWithPlace(secondPlaceUser.UserId, 2));
        podiumUserIds.Add(secondPlaceUser.UserId);
    }

    private static void AssignThirdPlace(
        List<IUserWithScore> sortedUsers,
        LeaderboardMinScores minScores,
        List<UserWithPlace> result,
        HashSet<long> podiumUserIds)
    {
        var thirdPlaceUser = sortedUsers.FirstOrDefault(u =>
            u.Score >= minScores.ThirdPlaceMinScore &&
            !podiumUserIds.Contains(u.UserId));
        if (thirdPlaceUser == null)
        {
            return;
        }
        
        result.Add(new UserWithPlace(thirdPlaceUser.UserId, 3));
        podiumUserIds.Add(thirdPlaceUser.UserId);
    }

    private static void AssignRemainingPlaces(
        List<IUserWithScore> sortedUsers,
        List<UserWithPlace> result,
        HashSet<long> podiumUserIds)
    {
        var nextPlace = FirstPlaceAfterPodium;
        var usersWithoutPodiumPlaces = sortedUsers
            .Where(user => !podiumUserIds.Contains(user.UserId));

        foreach (var user in usersWithoutPodiumPlaces)
        {
            result.Add(new UserWithPlace(user.UserId, nextPlace));
            nextPlace++;
        }
    }
}