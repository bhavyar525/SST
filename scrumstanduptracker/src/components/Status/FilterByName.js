import React, { useState } from "react";

const FilterByName = ({ onFilter }) => {
  const [name, setName] = useState("");

  const handleFilterClick = () => {
    if (!name.trim()) {
      alert("Please enter a developer name to filter.");
      return;
    }
    onFilter(name.trim());
  };

  return (
    <div>
<input
  type="text"
  placeholder="Enter Developer Name"
  value={name}
  onChange={(e) => setName(e.target.value)}
  style={{ flexGrow: 1, padding: "10px", fontSize: "16px", borderRadius: "5px", border: "1px solid #ccc" }}
/>
<button onClick={handleFilterClick} style={{ marginLeft: "10px", padding: "10px 16px", fontSize: "16px", cursor: "pointer", borderRadius: "5px" }}>
  Filter By Name
</button>

    </div>
  );
};

export default FilterByName;