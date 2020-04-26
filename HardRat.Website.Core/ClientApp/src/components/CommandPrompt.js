import React, { Component } from 'react';
import './CommandPrompt.css';

export class CommandPrompt extends Component {
    displayName = CommandPromt.name

    render() {
        return (
            <div>
                <h1>This is a client CommandPromt</h1>
                <p>Here you can type and execute commands like you would do in commandpromt on your own computer!</p>
                <p>port 1578!</p>
                <br></br>
                <p> If you skip this the application wont work!</p>

            </div>
        );
    }
}
