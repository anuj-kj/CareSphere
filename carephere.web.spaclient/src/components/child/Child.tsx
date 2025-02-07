import { useState } from "react";

interface ChildProps {
    data: string;
    sendData:(data: string)=>void;
}
const Child = ({ data, sendData }: ChildProps)=>{
    const [childData, setChildData]=useState(data);
    const handleInputChange=(e:React.ChangeEvent<HTMLInputElement>)=>{
        setChildData(e.target.value);        
    }
    const handleSendData=()=>{
        sendData(childData);
    }   
    return(
        <div>
            <h1>ParentChild</h1>
            <p>Data from Parent: {data}</p>
            <input type="text" value={childData} onChange={handleInputChange}/>
            <button onClick={handleSendData}>Send Data to Parent</button>
        </div>
    )

}
export default Child;
