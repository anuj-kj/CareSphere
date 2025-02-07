import { useState } from "react";
import Child from "../../components/child/Child";
const Parent=()=>{
    const [data]=useState('hello');
    const [childData, setChildData]=useState('');
    const handleData=(data:string)=>{
        setChildData(data);
    }

    return(
        <div>
            <h1>Parent</h1>
            <p>Data from Child: {childData}</p>
            <Child data={data} sendData={handleData} />
        </div>
    )
}
export default Parent;


