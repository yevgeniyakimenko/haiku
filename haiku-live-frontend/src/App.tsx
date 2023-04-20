import { useState, useEffect } from 'react'
import './App.css'

import { Author, Topic } from '../types'

import TopicPage from './components/TopicPage'

function App() {
  const [author, setAuthor] = useState<Author | null>(null)
  const [topics, setTopics] = useState<Topic[]>([])
  const [error, setError] = useState<string | null>(null)

  const handleSubmitAuthor = async (authorName: string) => {
    try {
      await fetch('/api/authors', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          "name": authorName
        }),
      })
        .then((response) => {
          if (response.ok) {
            return response.json()
          } else {
            setError('Error creating author. Please try again.')
            throw new Error('Error creating author.')
          }
        })
        .then((data) => {
          console.log('data:', data)
          setAuthor(data.author)
          setTopics(data.topics)
        })
    } catch (error) {
      console.log('error:', error)
    }
  }

  return (
    <div className="App h-full flex flex-col items-center justify-center sm:flex-row p-6">
      {author ? 
      <TopicPage author={author} topicList={topics} />
      : 
      <div className='flex flex-col items-center justify-center max-w-2xl h-full'>
        <h1 className='text-4xl font-semibold mb-4'>
          Welcome to Haiku Live
        </h1>

        <p className='text-xl mb-8'>
          A place to share your haikus with the world and enjoy the haikus of others.
        </p>

        <dl className='text-left w-full mb-8'>
          <dt>
            <span className='font-semibold text-xl'>Haiku</span> (n.)
          </dt>

          <dd>
            An unrhymed Japanese verse of three lines, often having a reference to season.
            Although the Japanese Haiku is traditionally written in a 5-7-5 syllable format,
            versions of Haiku in other languages do not necessarily follow this requirement.
          </dd>
        </dl>

        <form 
          className='flex flex-col items-start justify-center w-full sm:w-96'
          onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
            e.preventDefault()
            handleSubmitAuthor(
              e.currentTarget.authorName.value
            )
          }}
        >
          <p className='text-xl w-full text-left mb-4'>
            Please enter your name.
          </p>

          <label htmlFor="authorName" className='sm:mr-2 mb-2'>Your name:</label>
          <input
            autoFocus
            required
            maxLength={50}
            type="text"
            id="authorName"
            name="authorName"
            className='focus:outline-none bg-amber-300/10 placeholder:text-zinc-500 border rounded-md border-amber-900 w-full mb-4 sm:mr-4 px-4 py-2'
          />

          {/* <label htmlFor="username" className='sm:mr-2 mb-2'>Your short bio:</label>
          <textarea
            required
            maxLength={500}
            rows={4}
            id="bio"
            name="bio"
            className='focus:outline-none bg-amber-200/10 border rounded-md border-amber-900 w-full mb-4 sm:mr-4 px-4 py-2'
          /> */}

          <div className='flex justify-center w-full'>
            <button
              type="submit"
              className='focus:outline-none bg-amber-800 hover:bg-amber-700 border rounded-md border-amber-950 text-white font-bold mb-4 py-2 px-4'
              >
              Sign in
            </button>
          </div>

          {error && 
            <p 
              className='font-semibold text-red-700 bg-zinc-200/60 px-4 py-2 w-full mb-4 rounded-md'
            >
                {error}
            </p>
          }
        </form>
      </div>}
    </div>
  )
}

export default App
