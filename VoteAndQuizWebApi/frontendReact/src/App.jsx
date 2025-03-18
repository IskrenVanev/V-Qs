import React from 'react';
import {Route, Routes, Link } from 'react-router-dom';
import VoteList from './Votes/VoteList.jsx';
import About from './About.jsx';
import QuizList from './Quizzes/QuizList.jsx';
import Home from './Home.jsx';
import Register from './Identity/Register.jsx';
import Login from './Identity/Login.jsx';
import { useAuth } from './AuthContext.jsx'; // Import the useAuth hook
import LogoutLink from './Identity/LogoutLink.jsx';


function App() {
    const { isAuthenticated } = useAuth(); // Destructure the isAuthenticated state
    console.log(isAuthenticated);
    return (
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
                        {!isAuthenticated ? (
                            <>
                                <li className="nav-item">
                                    <Link to="/Login">Log In</Link>
                                </li>
                                <li className="nav-item">
                                    <Link to="/Register">Register</Link>
                                </li>
                            </>
                        ) : (
                            <>
                                <li className="nav-item">
                                    <LogoutLink>Log out</LogoutLink>
                                </li>
                            </>
                        )}
                    </ul>
                </nav>
            </header>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/About" element={<About />} />
                <Route path="/Votes" element={<VoteList />} />
                <Route path="/Quizzes" element={<QuizList />} />
                <Route path="/Login" element={<Login />} />
                <Route path="/Register" element={<Register />} />
            </Routes>
        </div>
    );
}

export default App;