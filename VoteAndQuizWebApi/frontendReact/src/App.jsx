import React from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import VoteList from './VoteList.jsx';
import About from './About.jsx';
import QuizList from './QuizList.jsx'
import Home from './Home.jsx';

function App() {
    return (
        <Router>
            <div className="App">
                <header className="app-header">
                    <h1 className="heading">V&Qs</h1>
                    <nav className="nav">
                        <ul className="nav-list">
                            <li className="nav-item">
                                <Link to="/">Home</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/About">About</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/Votes">Votes</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/Quizzes">Quizzes</Link>
                            </li>
                        </ul>
                    </nav>
                </header>
                <Routes>
                    <Route path="/" element={<Home />} /> 
                    <Route path="/About" element={<About />} />
                    <Route path="/Votes" element={<VoteList />} />
                    <Route path="/Quizzes" element={<QuizList />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;