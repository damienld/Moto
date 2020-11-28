select RiderName, SUM(ranking) as rankingtotal from (
(select RiderName, sum(RiderSessions.Rank) as SumRanks, count(RiderSessions.Rank) as NbQ2 
, SUM(CASE RiderSessions.Rank  
WHEN '1' THEN 25 
WHEN '2' THEN 20 
WHEN '3' THEN 16 
WHEN '4' THEN 13 
WHEN '5' THEN 11 
WHEN '6' THEN 10 
WHEN '7' THEN 9 
WHEN '8' THEN 8 
WHEN '9' THEN 7 
WHEN '10' THEN 6 
WHEN '11' THEN 5 
WHEN '12' THEN 4 
WHEN '13' THEN 3 
WHEN '14' THEN 2 
WHEN '15' THEN 1 
END) as ranking
from RiderSessions
left join Sessions as s on RiderSessions.Session_SessionId = s.SessionId
left join Gps as g on s.Gp_GpId = g.GpId
left join Seasons as sea on sea.SeasonId=g.GpId
where g.Season_SeasonId=3
and (s.SessionType=5)
group by RiderName

UNION

select RiderName, sum(RiderSessions.Rank+12) as SumRanks, count(RiderSessions.Rank) as NbQ1 
, SUM(CASE RiderSessions.Rank  
WHEN '3' THEN 3 
WHEN '4' THEN 2 
WHEN '5' THEN 1 
END) as ranking
from RiderSessions
left join Sessions as s on RiderSessions.Session_SessionId = s.SessionId
left join Gps as g on s.Gp_GpId = g.GpId
left join Seasons as sea on sea.SeasonId=g.GpId
where g.Season_SeasonId=3
and s.SessionType=4 and RiderSessions.Rank > 2
group by RiderName
)
) as Query1 group by RiderName


order by rankingtotal desc