import React, { Component } from 'react';
import { Canvas } from 'react-canvas-js'

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        Date.prototype.setISO8601 = function (string) {
            var regexp = "([0-9]{4})(-([0-9]{2})(-([0-9]{2})" +
                "(T([0-9]{2}):([0-9]{2})(:([0-9]{2})(\.([0-9]+))?)?" +
                "(Z|(([-+])([0-9]{2}):([0-9]{2})))?)?)?)?";
            var d = string.match(new RegExp(regexp));

            var offset = 0;
            var date = new Date(d[1], 0, 1);

            if (d[3]) { date.setMonth(d[3] - 1); }
            if (d[5]) { date.setDate(d[5]); }
            if (d[7]) { date.setHours(d[7]); }
            if (d[8]) { date.setMinutes(d[8]); }
            if (d[10]) { date.setSeconds(d[10]); }
            if (d[12]) { date.setMilliseconds(Number("0." + d[12]) * 1000); }
            if (d[14]) {
                offset = (Number(d[16]) * 60) + Number(d[17]);
                offset *= ((d[15] == '-') ? 1 : -1);
            }

            offset -= date.getTimezoneOffset();
            let time = (Number(date) + (offset * 60 * 1000));
            this.setTime(Number(time));
        }

        this.state = { agentResponses: [], loading: true };

        fetch('api/agents/responses')
            .then(response => response.json())
            .then(data => {
                this.setState({ agentResponses: data, loading: false });
            });
    }

    static renderTable(agentResponses) {
        return (
            <table className='table table-striped'>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Time</th>
                        <th>Name</th>
                        <th>A</th>
                        <th>B</th>
                        <th>C</th>
                    </tr>
                </thead>
                <tbody>
                    {agentResponses.map(agentResponse =>
                        <tr key={agentResponse.id}>
                            <td>{agentResponse.id}</td>
                            <td>{agentResponse.created_at}</td>
                            <td>{agentResponse.name}</td>
                            <td>{agentResponse.a}</td>
                            <td>{agentResponse.b}</td>
                            <td>{agentResponse.c}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    static renderChart(agentResponses) {
        let options = {
            axisX: {
                interval: 1,
                intervalType: "second"
            },
            data: [
                {
                    type: "line",
                    dataPoints: []
                }
            ]
        };

        agentResponses.forEach(m => {
            let date = new Date();
            date.setISO8601(m.created_at);
            options.data[0].dataPoints.push({
                x: date,
                y: (m.a + m.b + m.c) / 3
            })
        });

        console.log(options);

        return (<Canvas options={options} />);
    }

    render() {
        let content = <p><em>Loading...</em></p>;

        if (this.state.loading) {
            content = <p><em>Loading...</em></p>;
        } else {
            content = (
                <div>
                    {FetchData.renderTable(this.state.agentResponses)}
                    < br />
                    {FetchData.renderChart(this.state.agentResponses)}
                </div>
            );
        }

        return (
            <div>
                <h1>Agent responses</h1>
                {content}
            </div>
        );
    }
}
