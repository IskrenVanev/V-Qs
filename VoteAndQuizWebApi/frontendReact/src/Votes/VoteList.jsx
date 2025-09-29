import React, { useState } from 'react';
import axios from 'axios';
import './VoteList.css'; // Import the CSS file

const VoteList = () => {
    const [votes, setVotes] = useState([]);
    const [error, setError] = useState(null);
    const [showVotes, setShowVotes] = useState(false);

    const fetchVotes = () => {
        axios.get('https://localhost:7055/api/Votes', { withCredentials: true })
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
        <div className="vote-list-container">
            <h1>Votes</h1>
            <button className="show-votes-btn" onClick={fetchVotes}>Show Votes</button>
            {showVotes && (
                <div>
                    {error && <p className="error-message">Error: {error}</p>}
                    <ul className="vote-list">
                        {votes.map(vote => (
                            <li key={vote.id} className="balloon">{vote.name}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default VoteList;
