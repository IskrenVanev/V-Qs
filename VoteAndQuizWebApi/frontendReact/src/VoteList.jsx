import React, { useState, useEffect } from 'react';
import axios from 'axios';

const VoteList = () => {
    const [votes, setVotes] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        axios.get('https://localhost:7055/api/Votes')
            .then(response => {
                setVotes(response.data);
            })
            .catch(err => {
                setError(err.message);
            });
    }, []);

    return (
        <div>
            <h1>Votes</h1>
            {error && <p>Error: {error}</p>}
            <ul>
                {votes.map(vote => (
                    <li key={vote.id}>{vote.name}</li>
                ))}
            </ul>
        </div>
    );
};

export default VoteList;