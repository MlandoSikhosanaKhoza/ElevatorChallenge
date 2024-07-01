var Elevator = {
    Url: "https://localhost:7207",
    RenderTable: function () {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (xhttp.readyState == 4) {
                if (xhttp.status == 200) {
                    var buildingConfiguration = JSON.parse(xhttp.responseText);
                    var htmlTable = `<table class="border text-white">`;
                    for (var floor = 0; floor <= buildingConfiguration.NumOfFloors; floor++) {
                        htmlTable += "<tr>";
                        htmlTable += `<td class="text-black border">${(buildingConfiguration.NumOfFloors - floor)}</td>`;

                        for (var elevator = 0; elevator < buildingConfiguration.NumOfElevators; elevator++) {
                            htmlTable += `<td id="e${(elevator + 1)}-${(buildingConfiguration.NumOfFloors - floor)}" class="border elevator-room elevator-${(elevator + 1)}"></td>`;
                        }
                        htmlTable += `<td id="floor-${(buildingConfiguration.NumOfFloors - floor)}" class="passenger-${(buildingConfiguration.NumOfFloors - floor)}"></td>`;
                        htmlTable += "</tr>";
                    }

                    htmlTable += "</table>"
                    $("#elevator").html(htmlTable);
                    for (var i = 0; i < buildingConfiguration.Elevators.length; i++) {
                        var elevatorObj = buildingConfiguration.Elevators[i];
                        let elevatorId = elevatorObj.Id;
                        let currentFloor = elevatorObj.CurrentFloor;
                        let passengerCount = elevatorObj.PassengerCount;

                        $(`.elevator-${elevatorId}`).removeClass("bg-dark");
                        $(`.elevator-${elevatorId}`).text("");

                        $(`#e${elevatorId}-${currentFloor}`).addClass("bg-dark");
                        $(`#e${elevatorId}-${currentFloor}`).text(`${passengerCount}`);
                    }

                    for (const floor in buildingConfiguration.FloorRequestDictionary) {

                        var userQueue = buildingConfiguration.FloorRequestDictionary[floor];
                        let direction = "";
                        for (var i = 0; i < userQueue.length; i++) {
                            if (userQueue[i].CurrentFloor < userQueue[i].DestinationFloor)
                            {
                                direction = "UP TO";
                            } else {
                                direction = "DOWN TO";
                            }
                            document.getElementById(`floor-${floor}`)
                                .insertAdjacentHTML("beforeend", `<button class="button floor-${floor}">${userQueue[i].Passengers} waiting | ${direction} ${userQueue[i].DestinationFloor}</button>`);
                        }
                    }
                }
            }
        };
        xhttp.open("GET", `${Elevator.Url}/api/Building/GetBuilding`, true);
        xhttp.send();
    },
    InitializeElevatorStatusConnection: async function () {
        var connection = new signalR.HubConnectionBuilder()
            .withUrl(`${Elevator.Url}/elevator/status`)
            .build();

        await connection.start();

        connection.on('BroadcastStatus',
            function (elevatorId, currentFloor, passengerCount) {
                $(`.elevator-${elevatorId}`).removeClass("bg-dark");
                $(`.elevator-${elevatorId}`).text("");

                $(`#e${elevatorId}-${currentFloor}`).addClass("bg-dark");
                $(`#e${elevatorId}-${currentFloor}`).text(`${passengerCount}`);
            });
    },
    InitializePassengerStatusConnection: async function () {
        var connection = new signalR.HubConnectionBuilder()
            .withUrl(`${Elevator.Url}/passenger/status`)
            .build();

        await connection.start();

        connection.on('BroadcastPassengers',
            function (currentFloor, passengerJson) {
                var jsonList = JSON.parse(passengerJson);
                console.log(passengerJson);
                document.getElementById(`floor-${currentFloor}`).innerHTML = "";
                for (var i = 0; i < jsonList.length; i++) {
                    let direction = "";
                    if (currentFloor < jsonList[i].DestinationFloor) {
                        direction = "UP TO";
                    } else {
                        direction = "DOWN TO";
                    }
                    document.getElementById(`floor-${currentFloor}`)
                        .insertAdjacentHTML(
                            "beforeend",
                            `<button class="button floor-${currentFloor}">${jsonList[i].Passengers} waiting | ${direction} ${jsonList[i].DestinationFloor}</button>`);
                }
            });
        connection.on('DequeuePassengers',
            function (currentFloor) {
                var clsName = document.getElementsByClassName(`floor-${currentFloor}`);
                let lastElementIndex = clsName.length - 1;
                clsName[lastElementIndex].parentElement.removeChild(clsName[lastElementIndex]);
            });
    },
    RequestElevator: function () {
        var fd = new FormData(document.getElementById("request-form"));
        var url = new URL(`${Elevator.Url}/api/Building/RequestElevator`);
        url.searchParams.set("currentFloor", fd.get("CurrentFloor"));
        url.searchParams.set("destinationFloor", fd.get("DestinationFloor"));
        url.searchParams.set("passengers", fd.get("Passenger"));
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (xhttp.readyState == 4) {
                switch (xhttp.status) {
                    case 200:

                        break;
                    case 400:
                        var list = JSON.parse(xhttp.responseText);
                        var tableObj = document.createElement("table");
                        tableObj.style.textAlign = "left";
                        for (var i = 0; i < list.length; i++) {
                            var tr = document.createElement("tr");
                            tr.className = "border-bottom";
                            var td = document.createElement("td");
                            
                            td.textContent = list[i];

                            tr.appendChild(td);
                            tableObj.appendChild(tr);
                        }
                        Swal.fire({
                            icon: "error",
                            title: 'Error',
                            html: tableObj.outerHTML
                        });
                        break;
                    default:
                        break;
                }
            }
        };
        xhttp.open("GET", url.href, true);
        xhttp.send();
    }
};