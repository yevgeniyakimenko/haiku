import { useState } from "react"
import TopicLink from "./TopicLink"

import { Topic } from "../../types"

interface SidebarProps {
  topics: Topic[]
  selectedTopic: Topic
  onSelectTopic: (topic: Topic) => void
  onAddTopic: (input: string) => void
}

export default function Sidebar({ 
  topics, 
  selectedTopic, 
  onSelectTopic, 
  onAddTopic 
}: SidebarProps) {
  const [input, setInput] = useState('')

  return (
    <div className="sidebar text-left sm:w-1/4 pb-4 sm:pt-4 flex flex-col">
      <h1 className='text-2xl font-semibold text-orange-700 mb-3'>
        {'Haiku '}
        {/* <br className='hidden sm:inline' /> */}
        {'Live'}
      </h1>

      {/* a select for mobile */}
      {selectedTopic && <select
        value={selectedTopic.name}
        onChange={(e) => {
          const newSelTopic: Topic | undefined = topics.find((topic) => topic.id == parseInt(e.target.value))
          
          if (!newSelTopic) return
          onSelectTopic(newSelTopic)
        }}
        className='sm:hidden border bg-amber-200/75 rounded-md border-amber-950 px-4 py-3 mb-4'
      >
        {topics && topics.map((topic) => (
          <option key={topic.id} value={topic.name}>
            {topic.name}
          </option>
        ))}
      </select>}

      {/* a list for desktop */}
      <ul className='authorList hidden sm:block'>
        {topics && topics.map((topic) => (
          <TopicLink
            key={topic.id}
            topic={topic}
            selected={topic.id === selectedTopic.id}
            onClick={onSelectTopic}
          />
        ))}
      </ul>

      <form>
        <div className='flex sm:flex-col sm:items:'>
        <input
          type="text"
          maxLength={50}
          value={input}
          placeholder="New topic"
          onChange={(e) => setInput(e.target.value)}
          className='sm:block grow border rounded-md focus:outline-none bg-amber-300/10 placeholder:text-zinc-500 border-amber-900/50 px-4 py-2 mr-2 sm:mr-0 sm:mb-2'
        />
        <button
          type="button"
          className='text-white sm:w-16 border border-amber-900 rounded-md bg-amber-900 hover:bg-amber-700 px-3 py-2'
          onClick={async () => {
            onAddTopic(input)
            setInput('')
          }}
        >
          Add
        </button>
        </div>
      </form>
    </div>
  )
}
