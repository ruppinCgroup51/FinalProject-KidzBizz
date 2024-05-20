import React, { useEffect, useState } from 'react';

function UsersScores() {
    const [userScores, setUserScores] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            const setUserApi = () => {
                if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
                  return 'https://localhost:7034/api/UsersScores';
                } else {
                  return 'https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/UsersScores';
                }
              };

              const apiUrl = setUserApi();
            try {
                const response = await fetch(apiUrl); // Adjust the URL to your actual API endpoint
                const data = await response.json();
                data.sort((a, b) => b.Score - a.Score); // Sorting in descending order if not sorted by backend
                setUserScores(data);
            } catch (error) {
                console.error('Failed to fetch data:', error);
            }
        };
        
        fetchData();
    }, []);

    return (
        <div>
            {userScores.length > 0 ? (
                <ol>
                    {userScores.map((userScore, index) => (
                        <li key={index}>
                            {userScore.Username} - Score: {userScore.Score}
                        </li>
                    ))}
                </ol>
            ) : (
                <p>No user scores available.</p>
            )}
        </div>
    );
}

export default UsersScores;
