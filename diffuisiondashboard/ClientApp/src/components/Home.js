import React, { useEffect, useState } from 'react';
import ProgressBar from "@ramonak/react-progress-bar";
import { Chart } from "react-google-charts";

export function Home() {

    var [progress, setProgress] = useState(0);
    var [gpuUsed, setGpuUsed] = useState(0);
    var [gpuTotal, setGpuTotal] = useState(0);
    var [gpuTimeData, setGpuTimeData] = useState(
        [
                ["GPU", "Time"], [4, 5.5], [8, 12]
        ]);

    useEffect(() => {
        setInterval(() => {
            fetch(window.location.origin + "/api/diffusion/health")
                .then((res) => res.json())
                .then((json) => {
                    let used = Math.round(json.gpu.used);
                    let total = Math.round(json.gpu.total);
                    setGpuUsed(used);
                    setGpuTotal(total);
                    var subArray = [];
                    for (let i = 0; i < json.gpu.timeData.length; i++) {
                        subArray.push([i, json.gpu.timeData[i]]);
                    }
                    subArray.unshift(["GPU", "Time"]);
                    setGpuTimeData(subArray);
                    subArray = [];
                    console.log(subArray);
                });
        }, 1500);
        setProgress(progress+50);
    }, []);



    return (
        <div className="text-center">
            <h1 className="display-4">Diffusion AI Engine Dashboard</h1>
            <p>This dashboard helps monitor server health, perfomance and API hosting</p>
            <div className="alert alert-primary" role="alert">
                <b>API INTEGRATION GUIDE</b>
                <br />
                To access full fledged Swagger OpenAPI v3 documentation{" "}
                <a href="/swagger/index.html" className="alert-link">
                    click here
                </a>
            </div>
            <div className="row">

                <div className="col-sm-6">
                    <div className="card">
                        <div className="card-body">
                            <img
                                width="100px"
                                src="https://cdn-icons-png.flaticon.com/512/5723/5723045.png"
                            />
                            <h5 className="card-title">GPU Memory</h5>
                            <p className="card-text">
                                Live track GPU workload perfomance in Server
                            </p>
                            <h1 className="display-2">{Math.round(gpuUsed/1024 * 10)/10 + "GB / " + gpuTotal/1024 + "GB"}</h1>
                            <h4 className="card-title mb-5">GPU Preassure</h4>
                        </div>
                    </div>
                </div>

                <div className="col-sm-6">
                    <div className="card">
                        <div className="card-body">
                            <img
                                width="100px"
                                src="https://cdn-icons-png.flaticon.com/512/2704/2704265.png"
                            />
                            <h5 className="card-title">GPU Preassure</h5>
                            <p className="card-text">
                                GPU Dedicated Memory utilization on Server
                            </p>
                            <Chart
                                chartType="AreaChart"
                                data={gpuTimeData}
                                width="100%"
                                height="180px"
                                legendToggle
                            />
                        </div>
                    </div>
                </div>                                
            </div>
        </div>
    );
}
