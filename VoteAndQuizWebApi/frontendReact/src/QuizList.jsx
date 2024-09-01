import React, { useState } from 'react';
import axios from 'axios';

const VoteList = () => {
    const [quizzes, setQuizzes] = useState([]);
    const [error, setError] = useState(null);
    const [showQuizzes, setShowQuizzes] = useState(false);

    const fetchQuizzes = () => {
        axios.get('https://localhost:7055/api/Quizzes')
            .then(response => {
                setQuizzes(response.data);
                setShowQuizzes(true);
            })
            .catch(err => {
                setError(err.message);
                setShowQuizzes(true);
            });
    };

    return (
        <div>
            <h1>Quizzes</h1>
            <button onClick={fetchQuizzes}>Show Quizzes</button>
            {showQuizzes && (
                <div>
                    {error && <p>Error: {error}</p>}
                    <ul>
                        {quizzes.map(quiz => (
                            <li key={quiz.id}>{quiz.name}</li>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
};

export default VoteList;