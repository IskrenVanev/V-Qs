import React, { useState } from 'react';
import axios from 'axios';
import './VoteList.css'; // Import the CSS file

const VoteList = () => {
    const [votes, setVotes] = useState([]);
    const [chosenVote, setChosenVote] = useState(null);
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

    const openIframeWithVote = (id) => {
        axios.get(`https://localhost:7055/api/Votes/Details/${id}`, { withCredentials: true })
        .then(response => {
            setChosenVote(response.data);
        })
        .catch(err => {
            console.log("error fetching vote")
        })
    }

    return (
        <div className="vote-list-container">
            <h1>Votes</h1>
            <button className="show-votes-btn" onClick={fetchVotes}>Show Votes</button>
            {showVotes && (
                <div>
                    {/* {error && <p className="error-message">Error: {error}</p>} */}
                    {error && <p className="error-message">Error: Log in to see the Votes!</p>}
                    <ul className="vote-list">
                        {votes.map(vote => (
                            <li key={vote.id} 
                            className="balloon"
                            onClick={() => openIframeWithVote(vote.id)}
                            >
                                <strong>Name: {vote.name}</strong><br />    
                                <span>Ends in: {vote.voteEndDate}</span>
                                
                            </li>
                        ))}
                    </ul>
                    {/* Show vote only when it's selected */}
                    {chosenVote && (
                        <div className='iframe-container'>
                            <iframe 
                                src={`https://localhost:7055/api/Votes/Details/${chosenVote.id}`}
                                title="Vote Details"
                                width="600"
                                height="400"
                            />
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default VoteList;
