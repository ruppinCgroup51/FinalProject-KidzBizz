import React, { useEffect, useState } from 'react';

function UsersScores() {
    const [userScores, setUserScores] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('https://localhost:7034/api/UsersScores'); // Adjust the URL to your actual API endpoint
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
