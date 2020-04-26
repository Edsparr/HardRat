import React, { Component } from 'react';
import './Info.css';

export class Info extends Component {
    displayName = Info.name

    render() {
        return (
            <div>
                <h1>This is the information page</h1>
            <p>Before using the program, it is important that you port forward your router to</p>
            <p>port 1578!</p>
            <br></br>
<p> If you skip this the application wont work!</p>

            </div>
        );
    }
}
