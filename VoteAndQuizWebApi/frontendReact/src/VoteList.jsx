import React, { useState } from 'react';
import axios from 'axios';

const VoteList = () => {
    const [votes, setVotes] = useState([]);
    const [error, setError] = useState(null);
    const [showVotes, setShowVotes] = useState(false);

    const fetchVotes = () => {
        axios.get('https://localhost:7055/api/Votes')
            .then(response => {
                setVotes(response.data);
                setShowVotes(true);
            })
            .catch(err => {
                setError(err.message);
                setShowVotes(true);
            });
    };

    return (
        <div>
            <h1>Votes</h1>
            <button onClick={fetchVotes}>Show Votes</button>
            {showVotes && (
                <div>
                    {error && <p>Error: {error}</p>}
                    <ul>
                        {votes.map(vote => (
                            <li key={vote.id}>{vote.name}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default VoteList;