import React,{useState} from "react";
const FilterByDate = ({ onFilter }) => {
  const [date, setDate] = useState("");

  const handleSearch = () => {
    if (!date) {
      alert("Please select a date");
      return;
    }
    onFilter(date); // Send date to parent
  };

  return (
    <div style={{ display: "flex", alignItems: "center" }}>
      <input type="date" value={date} onChange={e => setDate(e.target.value)}   aria-label="Filter date input" />
      <button onClick={handleSearch}>Filter by Date</button>
    </div>
  );
};

export default FilterByDate;