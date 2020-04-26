 import React, { Component } from 'react';

export class FetchData extends Component {
  displayName = FetchData.name

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
    this.handleChange = this.handleChange.bind(this)
    this.handleAssembliesChange = this.handleAssembliesChange.bind(this)

  }
  handleChange(event) {
    this.setState({ codeInput: event.target.value });
  }

  handleAssembliesChange(event) {
    this.setState({ assemblies: event.target.value });
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1>Send code to client machine!</h1>
        <p>This component demonstrates fetching data from the server.</p>
        <textarea id="w3mission" rows="20" cols="50" value={this.state.codeInput} onChange={this.handleChange}>

        </textarea>

        <textarea id="w3mission" rows="20" cols="10" value={this.state.assemblies} onChange={this.handleAssembliesChange}>

        </textarea>


        <div>

        <button onClick={(event) => {
          fetch('api/ClientAction/Execute', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
              code: encodeURI(this.state.codeInput),
              assemblies: this.state.assemblies.split(' ')
            })
          })
          }}>Execute code on clients</button>

          <button onClick={(event) => {
            fetch('api/ClientAction/Action/ShowTaskbar', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
            })
          }}>Show taskbar</button>

          <button onClick={(event) => {
            fetch('api/ClientAction/Action/HideTaskbar', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
            })
          }}>Hide taskbar</button>

          
        </div>

      </div>
    );
  }
}
