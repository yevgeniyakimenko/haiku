import { useState, useEffect } from 'react'
import useSignalR from '../useSignalR'

import Sidebar from './Sidebar'
import HaikuElement from './Haiku'
import Form from './Form'

import { Author, Topic, Haiku, NewHaiku } from '../../types'


interface TopicPageProps {
  author: Author
  topicList: Topic[]
}

export default function TopicPage({ author, topicList }: TopicPageProps) {
  const { connection } = useSignalR('/r/topicHub')

  const [topics, setTopics] = useState<Topic[]>(topicList)
  const [selectedTopic, setSelectedTopic] = useState<Topic>(topics[0])
  const [haikus, setHaikus] = useState<Haiku[]>([])

  const handleSelectTopic = (topic: Topic) => {
    setSelectedTopic(topic)
  }
  
  const handleSubmitHaiku = async (haiku: NewHaiku) => {
    if (
      haiku.line1.length === 0 
      || haiku.line2.length === 0 
      || haiku.line3.length === 0
    ) return

    await fetch(`/api/topics/${selectedTopic.id}/haikus`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(haiku),
    })
  }

  const handleSubmitTopic = async (topicName: string) => {
    if (!topicName) return

    await fetch('/api/topics', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ name: topicName }),
    })
      .then((response) => response.json())
      .then((data) => {
        setTopics((topics) => [...topics, data])
      })
  }

  // get haikus for the selected topic
  useEffect(() => {
    if (!selectedTopic) return

    const fetchHaikus = async () => {
      const res = await fetch(`/api/topics/${selectedTopic.id}/haikus`)
      const json = await res.json()
      console.log('haikus json:', json)
      setHaikus(json)
    }
    fetchHaikus()
  }, [selectedTopic])

  useEffect(() => {
    if (!connection || !selectedTopic) return

    connection.invoke('AddToGroup', `${selectedTopic.id}`)

    connection.on('ReceiveMessage', (haiku) => {
      setHaikus((haikus) => [haiku, ...haikus])
    })

    return () => {
      connection.invoke('RemoveFromGroup', `${selectedTopic.id}`)
      connection.off('ReceiveMessage')
    }
  }, [connection, selectedTopic])

  return (
    <div className="Topic-Page h-full w-full sm:max-w-4xl flex flex-col sm:flex-row">
      <Sidebar 
        topics={topics}
        selectedTopic={selectedTopic}
        onSelectTopic={handleSelectTopic}
        onAddTopic={handleSubmitTopic}
      />

      <div className="main h-3/4 grow sm:h-full w-full flex flex-col sm:px-4">
        <div 
          id='HaikuListDiv' 
          className="Haikus grow overflow-y-auto w-full border rounded-md border-amber-900/20 p-4 mb-4 sm:mt-4"
        >
          <div id='HaikuList' className='text-left flex flex-wrap w-full'>
            {selectedTopic && haikus.map((haiku) => (
              <HaikuElement key={haiku.id} haiku={haiku} author={author} />
            ))}
          </div>
        </div>

        {selectedTopic && (
          <Form 
            authorId={author.id} 
            authorName={author.name}
            topicId={selectedTopic.id} 
            onSubmit={handleSubmitHaiku} 
          />
        )}
      </div>
    </div>
  )
}
